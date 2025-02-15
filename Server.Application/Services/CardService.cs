using AutoMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Server.Application.Common;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Contracts.DTO.Board;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Services
{
    public class CardService : ICardService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly ICardRepository _cardRepository;
        public CardService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService, IHttpContextAccessor contextAccessor, IEmailService emailService, IUserService userService, ICardRepository cardRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _emailService = emailService;
            _userService = userService;
            _cardRepository = cardRepository;
        }
        //public async Task<Pagination<ViewCardDTO>> ViewAllCardsPagination(int pageIndex, int pageSize)
        //{
        //    var totalItemsCount = await _unitOfWork.cardRepository.GetTotalCardCount(CardStatus.Open);
        //    var services = await _unitOfWork.cardRepository.GetPagedCards(pageIndex, pageSize, CardStatus.Open);
        //    var mappedCards = _mapper.Map<List<ViewCardDTO>>(services);

        //    return new Pagination<ViewCardDTO>
        //    {
        //        TotalItemsCount = totalItemsCount,
        //        PageSize = pageSize,
        //        PageIndex = pageIndex,
        //        Items = mappedCards
        //    };
        //}

    }
}
