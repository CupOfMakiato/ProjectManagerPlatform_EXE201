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
                Attachment = card.Attachment,
                ColumnId = card.ColumnId,
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


    }
}
