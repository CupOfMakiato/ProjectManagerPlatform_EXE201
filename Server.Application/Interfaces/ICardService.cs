using Microsoft.AspNetCore.Http;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface ICardService
    {
        Task<Result<object>> ViewAllCards();
        Task<Result<object>> ViewCardById(Guid cardId);
        Task<Result<object>> AddANewCard(AddCardDTO addCardDTO);
        Task<Result<object>> UpdateCard(UpdateCardDTO updateCardDTO);
        Task<Result<object>> ChangeCardName(ChangeCardNameDTO changeCardNameDTO);
        Task<Result<object>> DeleteCard(Guid cardId);
        Task<Result<object>> UploadFileAttachment(Guid cardId, IFormFile file);
    }
}
