using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using Net.payOS;
using Newtonsoft.Json;
using Server.Application.Interfaces;
using Server.Contracts.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Server.Application;
using Server.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Server.Contracts.Abstractions.Shared;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Server.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PayOS _payOS;
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;
        private readonly string _domain;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IConfiguration configuration, IUnitOfWork unitOfWork, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            _clientId = configuration["PayOS:ClientId"];
            _apiKey = configuration["PayOS:APIKey"];
            _checksumKey = configuration["PayOS:ChecksumKey"];
            _domain = configuration["PayOS:Domain"];
            _payOS = new PayOS(_clientId, _apiKey, _checksumKey);
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreatePaymentLinkAsync(CreatePaymentRequest createPaymentRequest)
        {
            var getSubcription = await _unitOfWork.subcriptionRepository.GetByIdAsync(createPaymentRequest.SubcriptionId);
            var existedSubcribe = await _unitOfWork.subcribeRepository.CheckExist(createPaymentRequest.SubcriptionId, createPaymentRequest.UserId);
            if (getSubcription == null)
            {
                return "Subcription is not exist";
            }
            if(existedSubcribe != null)
            {
                return "You have purchased this service package";
            }
            int code = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            var returnUrl = $"{_domain}api/payment/result?orderCode={code}&status=return";
            var cancelUrl = $"{_domain}api/payment/result?orderCode={code}&status=cancel";
            var paymentData = new PaymentData(
                orderCode: code,
                amount: (int)getSubcription.Price,
                description: $"Thanh toán mã {code}",
                items: [new($"Mua gói {getSubcription.SubcriptionName.ToString()}", 1, (int)getSubcription.Price)],
                returnUrl: returnUrl,
                cancelUrl: cancelUrl
            );
            _memoryCache.Set($"SUBCRIPTION_{code}", createPaymentRequest.SubcriptionId, TimeSpan.FromMinutes(30));
            _memoryCache.Set($"USER_{code}", createPaymentRequest.UserId, TimeSpan.FromMinutes(30));
            _memoryCache.Set($"PRICE_{code}", paymentData.amount, TimeSpan.FromMinutes(30));
            var response = await _payOS.createPaymentLink(paymentData);
            _memoryCache.Set($"CHECKOUTURL_{code}", response.checkoutUrl, TimeSpan.FromMinutes(30));
            return response.checkoutUrl;
        }

        public async Task<string> CheckPaymentStatusAsync(int orderCode)
        {
            var paymentStatus = await _payOS.getPaymentLinkInformation(orderCode);

            if (_memoryCache.TryGetValue($"SUBCRIPTION_{orderCode}", out Guid subcriptionId) && _memoryCache.TryGetValue($"USER_{orderCode}", out Guid userId) && _memoryCache.TryGetValue($"PRICE_{orderCode}", out int price) && _memoryCache.TryGetValue($"CHECKOUTURL_{orderCode}", out string checkoutUrl))
            {
                if (paymentStatus.status == "PAID")
                {
                    var getSubcription = await _unitOfWork.subcriptionRepository.GetByIdAsync(subcriptionId);
                    if (getSubcription == null)
                        return "Subcription not found in DB";
                    var existedSubcribe = await _unitOfWork.subcribeRepository.CheckExist(subcriptionId, userId);

                    if (existedSubcribe == null)
                    {
                        var subcribe = new Subcribe
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddDays(getSubcription.Duration),
                            SubcriptionId = subcriptionId,
                            UserId = userId,
                            CreationDate = DateTime.UtcNow,
                            CreatedBy = userId,
                            IsDeleted = false
                        };
                        await _unitOfWork.subcribeRepository.AddSubcribeAsync(subcribe);
                        var payment = new Payment
                        {
                            UserId = userId,
                            TotalAmount = price,
                            SubcriptionId = subcriptionId,
                            PaymentUrl = checkoutUrl,
                            PaymentStatus = paymentStatus.status,
                            PaymentDate = DateTime.UtcNow,
                            PaymentMethod = "ONLINE",
                            TransactionId = orderCode.ToString(),
                            CreationDate = DateTime.UtcNow,
                            CreatedBy = userId,
                            IsDeleted = false
                        };
                        await _unitOfWork.paymentRepository.AddPaymentAsync(payment);
                    }

                    return paymentStatus.status;
                }
                return paymentStatus.status;
            }
            return "Missing data from cache";

        }
    }
}
