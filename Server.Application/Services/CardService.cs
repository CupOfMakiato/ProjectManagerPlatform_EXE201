using AutoMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Server.Application.Common;
using Server.Application.Interfaces;
using Server.Application.Mappers.BoardExtension;
using Server.Application.Mappers.CardExtension;
using Server.Application.Repositories;
using Server.Contracts.Abstractions.CloudinaryService;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Card;
using Server.Domain.Entities;
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
        private readonly ICloudinaryService _cloudinaryService;
        public CardService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService, IHttpContextAccessor contextAccessor, IEmailService emailService, IUserService userService, ICardRepository cardRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _emailService = emailService;
            _userService = userService;
            _cardRepository = cardRepository;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<Result<object>> ViewAllCards()
        {
            var cards = await _unitOfWork.cardRepository.GetAllOpenCards();

            var result = cards.Select(card => card.ToViewCardDTO()).ToList();

            return new Result<object>
            {
                Error = result.Any() ? 0 : 1,
                Message = result.Any() ? "Cards retrieved successfully" : "No open cards found",
                Data = result
            };
        }

        public async Task<Result<object>> ViewCardById(Guid cardId)
        {
            ViewCardDTO result = null;
            var card = await _unitOfWork.cardRepository.GetCardById(cardId);
            if (card != null)
                result = card.ToViewCardDTO();
            return new Result<object>
            {
                Error = result != null ? 0 : 1,
                Message = result != null ? "Get card successfully" : "Get card fail",
                Data = result
            };
        }

        public async Task<Result<object>> AddANewCard(AddCardDTO addCardDTO)
        {
            var user = await _unitOfWork.userRepository.GetByIdAsync(addCardDTO.UserId);
            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "User does not exist!",
                    Data = null
                };
            }
            var cardMapper = addCardDTO.ToCard();

            await _unitOfWork.cardRepository.AddAsync(cardMapper);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Add new card successfully" : "Add new card fail",
                Data = null
            };
        }

        public async Task<Result<object>> UploadFileAttachment(Guid cardId, IFormFile file)
        {
            // Validate file input
            if (file == null || file.Length == 0)
            {
                return new Result<object> { Error = 1, Message = "File is empty?" };
            }

            // Fetch the card
            var card = await _cardRepository.GetCardById(cardId);
            if (card == null)
            {
                return new Result<object> { Error = 1, Message = "Card not found" };
            }

            // Extract file details
            string fileName = file.FileName;
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            bool isImage = fileExtension == ".png" || fileExtension == ".jpeg" || fileExtension == ".jpg" || fileExtension == ".gif";

            // Upload to Cloudinary
            CloudinaryResponse uploadResult = isImage
                ? await _cloudinaryService.UploadCardImage(fileName, file, card)
                : await _cloudinaryService.UploadCardFile(fileName, file, card);

            if (uploadResult == null)
            {
                return new Result<object> { Error = 1, Message = "Something went wrong, file upload failed!" };
            }

            // Determine if this should be the cover
            bool shouldBeCover = isImage && !card.Attachments.Any(a => a.IsCover);

            // Create attachment
            var attachment = new Attachment
            {
                FileName = fileName,
                FileUrl = uploadResult.FileUrl ?? "",
                FileType = file.ContentType,
                FilePublicId = uploadResult.PublicFileId ?? "",
                CreationDate = DateTime.Now,
                IsCover = shouldBeCover,
                CardId = card.Id
            };

            // Add attachment and save changes
            card.Attachments.Add(attachment);
            _cardRepository.Update(card);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "File uploaded successfully",
                Data = attachment
            };
        }


    }
}
