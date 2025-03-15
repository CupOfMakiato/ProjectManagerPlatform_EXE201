using AutoMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Server.Application.Common;
using Server.Application.Interfaces;
using Server.Application.Mappers.BoardExtension;
using Server.Application.Mappers.CardExtension;
using Server.Application.Repositories;
using Server.Contracts.Abstractions.CloudinaryService;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Card;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IColumnRepository _columnRepository;
        private readonly ICloudinaryService _cloudinaryService;
        public CardService(IUnitOfWork unitOfWork, IMapper mapper,
            ICloudinaryService cloudinaryService, IHttpContextAccessor contextAccessor,
            IEmailService emailService, IUserService userService, ICardRepository cardRepository,
            IColumnRepository columnRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _emailService = emailService;
            _userService = userService;
            _cardRepository = cardRepository;
            _cloudinaryService = cloudinaryService;
            _columnRepository = columnRepository;
        }
        public async Task<Result<object>> ViewAllCards()
        {
            var cards = await _unitOfWork.cardRepository.GetAllCards();

            var result = cards.Select(card => card.ToViewCardDTO()).ToList();

            return new Result<object>
            {
                Error = result.Any() ? 0 : 1,
                Message = result.Any() ? "Cards retrieved successfully" : "No open cards found",
                Data = result
            };
        }

        public async Task<Result<object>> ViewAllOpenCards()
        {
            var cards = await _unitOfWork.cardRepository.GetAllOpenCards();
            var result = cards.Select(card => card.ToViewCardDTO()).ToList();
            return new Result<object>
            {
                Error = result.Any() ? 0 : 1,
                Message = result.Any() ? "Open cards retrieved successfully" : "No open cards found",
                Data = result
            };
        }

        public async Task<Result<object>> ViewAllArchivedCards()
        {
            var cards = await _unitOfWork.cardRepository.GetAllArchivedCards();
            var result = cards.Select(card => card.ToViewCardDTO()).ToList();
            return new Result<object>
            {
                Error = result.Any() ? 0 : 1,
                Message = result.Any() ? "Archived cards retrieved successfully" : "No archived cards found",
                Data = result
            };
        }

        public async Task<Result<object>> ArchiveCard(Guid cardId)
        {
            var existingCard = await _unitOfWork.cardRepository.GetCardById(cardId);
            if (existingCard == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card not found",
                    Data = null
                };
            }

            if (existingCard.Status == CardStatus.Closed)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card is already archived",
                    Data = null
                };
            }

            existingCard.Status = CardStatus.Closed;
            _unitOfWork.cardRepository.Update(existingCard);

            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Card archived successfully" : "Failed to archive card",
                Data = existingCard
            };
        }


        public async Task<Result<object>> UnarchiveCard(Guid cardId)
        {
            var existingCard = await _unitOfWork.cardRepository.GetCardById(cardId);
            if (existingCard == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card not found",
                    Data = null
                };
            }

            if (existingCard.Status != CardStatus.Closed)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card is already opened",
                    Data = null
                };
            }

            existingCard.Status = CardStatus.Open;

            _unitOfWork.cardRepository.Update(existingCard);

            // Save the changes
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Card unarchived successfully" : "Failed to unarchive card",
                Data = existingCard
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

            // Fetch all cards in the same list
            var cardsInList = await _unitOfWork.cardRepository.GetCardsByColumnId(addCardDTO.ColumnId);

            // Check if the list has reached the max card limit
            if (cardsInList.Count >= 25)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Cannot add more than 25 cards in a list",
                    Data = null
                };
            }

            // Determine new card position (max position + 1), starting from 1 if empty
            int newPosition = cardsInList.Any() ? cardsInList.Max(c => c.CardPosition) + 1 : 1;

            var newCard = addCardDTO.ToCard();
            newCard.CardPosition = newPosition; // Assign the new position

            await _unitOfWork.cardRepository.AddAsync(newCard);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "New card added successfully" : "Failed to add card",
                Data = newCard
            };
        }


        public async Task<Result<object>> UpdateCard(UpdateCardDTO updateCardDTO)
        {
            var card = await _unitOfWork.cardRepository.GetCardById(updateCardDTO.Id);
            if (card == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card not found",
                    Data = null
                };
            }

            card.Title = updateCardDTO.Title;
            card.Description = updateCardDTO.Description;
            card.Status = updateCardDTO.Status;
            card.AssignedCompletion = updateCardDTO.AssignedCompletion;

            _unitOfWork.cardRepository.Update(card);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Update card successfully" : "Update card failed",
                Data = null
            };
        }

        public async Task<Result<object>> ChangeCardName(ChangeCardNameDTO changeCardNameDTO)
        {
            var card = await _unitOfWork.cardRepository.GetCardById(changeCardNameDTO.Id);
            if (card == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card not found",
                    Data = null
                };
            }

            card.Title = changeCardNameDTO.Title;

            _unitOfWork.cardRepository.Update(card);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Change Card Name successfully" : "Change card name failed",
                Data = null
            };
        }

        public async Task<Result<object>> EditCardDescription(EditCardDescriptionDTO editCardDescriptionDTO)
        {
            var card = await _unitOfWork.cardRepository.GetCardById(editCardDescriptionDTO.Id);
            if (card == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card not found",
                    Data = null
                };
            }

            card.Description = editCardDescriptionDTO.Description;

            _unitOfWork.cardRepository.Update(card);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Edit successfully" : "Edit failed",
                Data = null
            };
        }

        public async Task<Result<object>> DeleteCard(Guid cardId)
        {
            // Retrieve the existing card
            var existingCard = await _unitOfWork.cardRepository.GetCardById(cardId);
            if (existingCard == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card not found",
                    Data = null
                };
            }

            // Soft delete the card
            _unitOfWork.cardRepository.SoftRemove(existingCard);

            // Save the changes
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Card deleted successfully" : "Failed to delete Card",
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
            bool isImage = fileExtension == ".png" 
                || fileExtension == ".jpeg" 
                || fileExtension == ".jpg" 
                || fileExtension == ".gif"
                || fileExtension == ".webp";

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

        public async Task<Result<object>> DeleteAttachment(Guid cardId, Guid fileId)
        {
            // Fetch the card
            var card = await _cardRepository.GetCardById(cardId);
            if (card == null)
            {
                return new Result<object> { Error = 1, Message = "Card not found" };
            }

            // Find the attachment
            var file = card.Attachments.FirstOrDefault(a => a.Id == fileId);
            if (file == null)
            {
                return new Result<object> { Error = 1, Message = "File not found" };
            }

            // Delete from Cloudinary
            var deleteResult = await _cloudinaryService.DeleteFileAsync(file.FilePublicId);
            if (deleteResult == null || (deleteResult.Result != "ok" && deleteResult.Result != "not found"))
            {
                return new Result<object> { Error = 1, Message = "There was an issue while deleting..." };
            }

            // Remove the attachment from the card
            card.Attachments.Remove(file);

            // If the deleted attachment was the cover, assign a new cover

            if (file.IsCover)
            {
                var newCover = card.Attachments.FirstOrDefault(a => a.FileType.Contains("image"));
                if (newCover != null)
                {
                    newCover.IsCover = true;
                }
            }

            // Save changes
            _cardRepository.Update(card);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "File deleted successfully" : "Failed to delete file",
                Data = null
            };
        }


        public async Task<Result<object>> MoveCardInColumn(MoveCardInColumnDTO moveCardInColumnDTO)
        {
            var card = await _unitOfWork.cardRepository.GetCardById(moveCardInColumnDTO.Id);
            if (card == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card not found",
                    Data = null
                };
            }

            // Ensure the card belongs to the specified column
            if (card.ColumnId != moveCardInColumnDTO.ColumnId)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card does not belong to the this List",
                    Data = null
                };
            }

            // Fetch all open cards in the column (ordered by position)
            var cards = await _unitOfWork.cardRepository.GetOpenCardsByColumnId(moveCardInColumnDTO.ColumnId);

            // Validate that the new position is within the allowed range
            // Allow moving to the last position (new position == cards.Count)
            if (moveCardInColumnDTO.NewPosition < 1 || moveCardInColumnDTO.NewPosition > cards.Count)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Invalid position!",
                    Data = null
                };
            }

            // Ensure position starts from 1 instead of 0
            cards = cards.OrderBy(c => c.CardPosition).ToList(); // Sort the cards by position
            cards.Remove(card); 
            cards.Insert(moveCardInColumnDTO.NewPosition - 1, card); // Insert the card at the new position 

            // Reassign positions sequentially 
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].CardPosition = i + 1; // positioning from 1 instead of 0
                _unitOfWork.cardRepository.Update(cards[i]); 
            }

            // Save changes to the database
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Card moved successfully",
                Data = card
            };
        }

        public async Task<Result<object>> MoveCardToList(MoveCardToColumnDTO moveCardToListDTO)
        {
            // Fetch the card to be moved
            var card = await _unitOfWork.cardRepository.GetCardById(moveCardToListDTO.Id);
            if (card == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card not found",
                    Data = null
                };
            }

            // Check if the new column exists
            var newColumn = await _unitOfWork.columnRepository.GetColumnsById(moveCardToListDTO.NewColumnId);
            if (newColumn == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Target column not found",
                    Data = null
                };
            }

            // Fetch all cards in the current and new lists
            var currentColumnCards = await _unitOfWork.cardRepository.GetOpenCardsByColumnId(card.ColumnId);
            var newColumnCards = await _unitOfWork.cardRepository.GetOpenCardsByColumnId(moveCardToListDTO.NewColumnId);

            // Allow inserting at the last position
            if (moveCardToListDTO.NewPosition < 1 || moveCardToListDTO.NewPosition > newColumnCards.Count + 1)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Invalid position in target column",
                    Data = null
                };
            }

            // Remove the card from the current list
            currentColumnCards = currentColumnCards.OrderBy(c => c.CardPosition).ToList();
            currentColumnCards.Remove(card);

            // Reassign positions in the current list
            for (int i = 0; i < currentColumnCards.Count; i++)
            {
                currentColumnCards[i].CardPosition = i + 1; // Ensure 1-based positions
                _unitOfWork.cardRepository.Update(currentColumnCards[i]);
            }

            // Move the card to the new list and insert at the specified position
            newColumnCards = newColumnCards.OrderBy(c => c.CardPosition).ToList();
            card.ColumnId = moveCardToListDTO.NewColumnId; // Update the column ID
            newColumnCards.Insert(moveCardToListDTO.NewPosition - 1, card); // Adjust to 1-based index

            // Reassign positions in the new list
            for (int i = 0; i < newColumnCards.Count; i++)
            {
                newColumnCards[i].CardPosition = i + 1;
                _unitOfWork.cardRepository.Update(newColumnCards[i]);
            }

            // Save all changes
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Card moved successfully to the new column",
                Data = card
            };
        }

        public async Task<Result<object>> DownloadAttachment(Guid cardId, Guid attachmentId)
        {
            // fetch card
            var card = await _cardRepository.GetCardById(cardId);
            if (card == null)
            {
                return new Result<object> { Error = 1, Message = "Card not found!" };
            }

            // choose attachment
            var attachment = card.Attachments.FirstOrDefault(a => a.Id == attachmentId);
            if (attachment == null)
            {
                return new Result<object> { Error = 1, Message = "File not found!" };
            }

            return new Result<object>
            {
                Error = 0,
                Message = "File retrieved successfully",
                Data = attachment.ToDownloadAttachmentDTO()
            };
        }

        public async Task<Result<object>> AddDueDateToCard(AddDueDateToCardDTO addDueDateDTO)
        {
            var card = await _unitOfWork.cardRepository.GetCardById(addDueDateDTO.CardId);
            if (card == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Card not found",
                    Data = null
                };
            }

            // Ensure DueDate is in the future
            if (addDueDateDTO.DueDate < DateTime.UtcNow)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Due date must be in the future",
                    Data = null
                };
            }

            // Convert StartDate (DateOnly) to DateTime manually
            if (addDueDateDTO.StartDate.HasValue)
            {
                card.StartDate = new DateTime(
                    addDueDateDTO.StartDate.Value.Year,
                    addDueDateDTO.StartDate.Value.Month,
                    addDueDateDTO.StartDate.Value.Day,
                    0, 0, 0, DateTimeKind.Utc 
                );
            }

            // Store DueDate with both date & time in UTC
            card.DueDate = DateTime.SpecifyKind(addDueDateDTO.DueDate, DateTimeKind.Utc);

            // Assign Reminder
            card.Reminder = addDueDateDTO.Reminder;

            _unitOfWork.cardRepository.Update(card);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Due date added successfully" : "Failed to add due date",
                Data = new
                {
                    addDueDateDTO.CardId,
                    StartDate = card.StartDate?.ToString("yyyy-MM-dd"), 
                    DueDate = card.DueDate?.ToString("yyyy-MM-dd HH:mm"), 
                    Reminder = card.Reminder
                }
            };
        }



    }
}
