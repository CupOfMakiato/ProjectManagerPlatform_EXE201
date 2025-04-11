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

namespace Server.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly PayOS _payOS;
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;
        private readonly string _baseUrl = "https://api.payos.vn/";
        private readonly string _domain = "https://localhost:3030/";

        public PaymentService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _clientId = configuration["PayOS:ClientId"];
            _apiKey = configuration["PayOS:APIKey"];
            _checksumKey = configuration["PayOS:ChecksumKey"];
            _payOS = new PayOS(_clientId, _apiKey, _checksumKey);
        }

        public async Task<string> CreatePaymentLinkAsync(CreatePaymentRequest createPaymentRequest)
        {
            int code = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            var paymentData = new PaymentData(
                orderCode: code,
                amount: 5000,
                description: $"Thanh toán mã {code}",
                items: [new("Mì tôm hảo hảo ly", 1, 5000)],
                returnUrl: _domain,
                cancelUrl: _domain
            );

            var response = await _payOS.createPaymentLink(paymentData);
            return response.checkoutUrl;
        }
    }
}
