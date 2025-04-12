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
        // View Cards
        Task<Result<object>> ViewAllCards();
        Task<Result<object>> ViewAllOpenCards();
        Task<Result<object>> ViewAllArchivedCards();
        Task<Result<object>> ViewCardById(Guid cardId);

        // Update status
        Task<Result<object>> UnarchiveCard(Guid cardId);
        Task<Result<object>> ArchiveCard(Guid cardId);


        // Create Cards
        Task<Result<object>> AddANewCard(AddCardDTO addCardDTO);

        // Update string
        Task<Result<object>> UpdateCard(UpdateCardDTO updateCardDTO);
        Task<Result<object>> ChangeCardName(ChangeCardNameDTO changeCardNameDTO);
        Task<Result<object>> EditCardDescription(EditCardDescriptionDTO editCardDescriptionDTO);

        //Add due date
        Task<Result<object>> AddDueDateToCard(AddDueDateToCardDTO addDueDateDTO);
        // Delete cards
        Task<Result<object>> DeleteCard(Guid cardId);

        // Upload files
        Task<Result<object>> UploadFileAttachment(Guid cardId, IFormFile file);

        Task<Result<object>> DeleteAttachment(Guid cardId, Guid fileId);

        // Move cards
        Task<Result<object>> MoveCardInColumn(MoveCardInColumnDTO moveCardInColumnDTO);
        Task<Result<object>> MoveCardToList(MoveCardToColumnDTO moveCardToListDTO);

        // Download file
        Task<Result<object>> DownloadAttachment(Guid cardId, Guid fileId);
    }
}
