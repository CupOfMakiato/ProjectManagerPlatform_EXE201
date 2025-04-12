using Server.Application.Mappers.AttachmentExtension;
using Server.Application.Mappers.ColumsExtensions;
using Server.Application.Mappers.UserExtension;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Card;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Mappers.CardExtension
{
    public static class CardExtensions
    {
        public static ViewCardDTO ToViewCardDTO(this Card card)
        {
            return new ViewCardDTO
            {
                Id = card.Id,
                Title = card.Title,
                Description = card.Description,
                CardPosition = card.CardPosition,
                Column = card.Column.ToViewColumnDTO(),
                Attachments = card.Attachments.Select(a => a.ToViewAttachmentDTO()).ToList(),
                Status = (Domain.Enums.CardStatus)card.Status,
                AssignedCompletion = (Domain.Enums.AssignedCompletion)card.AssignedCompletion,
                CreatedByUser = card.CardCreatedByUser.ToUserDTO()

            };
        }
        public static Card ToCard(this AddCardDTO addCardDTO)
        {
            return new Card
            {
                Id = addCardDTO.Id,
                ColumnId = addCardDTO.ColumnId,
                Title = addCardDTO.Title,
                AssignedCompletion = addCardDTO.AssignedCompletion,
                Status = addCardDTO.Status,
                CreatedBy = addCardDTO.UserId

            };
        }

        public static AddCardDTO ToAddCardDTO(this AddNewCardRequest addNewCardRequest)
        {
            return new AddCardDTO
            {
                Id = (Guid)addNewCardRequest.Id,
                UserId = addNewCardRequest.UserId,
                ColumnId = addNewCardRequest.ColumnId,
                Title = addNewCardRequest.Title,
                Status = CardStatus.Open,
                AssignedCompletion = AssignedCompletion.Incomplete,

            };
        }

        public static UpdateCardDTO ToUpdateCardDTO(this UpdateCardRequest updateCardRequest)
        {
            return new UpdateCardDTO
            {
                Id = (Guid)updateCardRequest.Id,
                Title = updateCardRequest.Title,
                Description = updateCardRequest.Description,
                Status = updateCardRequest.Status,
                AssignedCompletion = updateCardRequest.AssignedCompletion,
            };
        }

        public static ChangeCardNameDTO ToChangeCardNameDTO(this ChangeCardNameRequest changeCardNameRequest)
        {
            return new ChangeCardNameDTO
            {
                Id = (Guid)changeCardNameRequest.Id,
                Title = changeCardNameRequest.Title,
            };
        }

        public static EditCardDescriptionDTO ToEditCardDescriptionDTO(this EditCardDescriptionRequest editCardDescriptionRequest)
        {
            return new EditCardDescriptionDTO
            {
                Id = (Guid)editCardDescriptionRequest.Id,
                Description = editCardDescriptionRequest.Description,
            };
        }

        public static MoveCardInColumnDTO ToMoveCardInColumnDTO(this MoveCardInColumnRequest moveCardInColumnRequest)
        {
            return new MoveCardInColumnDTO
            {
                Id = (Guid)moveCardInColumnRequest.Id,
                ColumnId = moveCardInColumnRequest.ColumnId,
                NewPosition = moveCardInColumnRequest.NewPosition,
            };
        }

        public static MoveCardToColumnDTO ToMoveCardToColumnDTO(this MoveCardToColumnRequest moveCardToListRequest)
        {
            return new MoveCardToColumnDTO
            {
                Id = (Guid)moveCardToListRequest.Id,
                NewColumnId = moveCardToListRequest.NewColumnId,
                NewPosition = moveCardToListRequest.NewPosition,
            };
        }

        public static DownloadAttachmentDTO ToDownloadAttachmentDTO(this Attachment attachment)
        {
            return new DownloadAttachmentDTO
            {
                FileName = attachment.FileName,
                FileUrl = attachment.FileUrl
            };
        }

        public static AddDueDateToCardDTO ToAddDueDateToCardDTO(this AddDueDateToCardRequest addDueDateToCardRequest)
        {
            return new AddDueDateToCardDTO
            {
                CardId = (Guid)addDueDateToCardRequest.CardId,
                StartDate = addDueDateToCardRequest.StartDate,
                DueDate = addDueDateToCardRequest.DueDate,
                Reminder = addDueDateToCardRequest.Reminder
            };
        }

    }
}
