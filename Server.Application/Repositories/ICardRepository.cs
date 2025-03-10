using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Repositories
{
    public interface ICardRepository : IGenericRepository<Card>
    {
        Task<int> GetTotalCardCount(CardStatus? status = null);
        Task<int> CountAttachmentsInACard(Guid cardId);
        Task<List<Card>> GetAllCards();
        Task<List<Card>> GetAllOpenCards();
        Task<List<Card>> GetAllArchivedCards();
        Task<Card?> GetCardById(Guid id);
        Task<List<Card>> GetPagedCards(int pageIndex, int pageSize, CardStatus? status = null);
        Task<List<Card>> SearchCardsAsync(string textSearch);
        Task<List<Card>> GetCardsByBoardId(Guid boardId);
        Task<List<Card>> GetCardsByColumnId(Guid columnId);
        Task<List<Card>> GetOpenCardsByColumnId(Guid columnId);
        Task<List<Card>> GetArchivedCardsByColumnId(Guid columnId);


        //filter
    }
}
 