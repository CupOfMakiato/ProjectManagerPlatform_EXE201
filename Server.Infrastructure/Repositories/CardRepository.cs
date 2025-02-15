using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Repositories
{
    public class CardRepository : GenericRepository<Card>, ICardRepository
    {
        private readonly AppDbContext _dbContext;

        public CardRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<int> GetTotalCardCount(CardStatus? status = null)
        {
            var query = _dbContext.Cards.Where(c => !c.IsDeleted);

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            return await query.CountAsync();
        }

        public async Task<List<Card>> GetAllCards()
        {
            return await _dbContext.Cards.ToListAsync();
        }

        public async Task<List<Card>> GetAllOpenCards()
        {
            return await _dbContext.Cards.Where(c => c.Status != CardStatus.Open).ToListAsync();
        }

        public async Task<List<Card>> GetAllArchivedCards()
        {
            return await _dbContext.Cards.Where(c => c.Status != CardStatus.Closed).ToListAsync();
        }

        public async Task<Card> GetCardById(Guid id)
        {
            return await _dbContext.Cards.Where(c => c.Id == id).Include(c => c.CardCreatedByUser).FirstOrDefaultAsync();
        }
        public async Task<List<Card>> GetPagedCards(int pageIndex, int pageSize, CardStatus? status = null)
        {
            var query = _dbContext.Cards
                .Where(c => !c.IsDeleted);

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            return await query
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(s => s.CardCreatedByUser)
                .ToListAsync();
        }

        public async Task<List<Card>> SearchCardsAsync(string textSearch)
        {
            return await _dbContext.Cards
                .Where(s => s.Title.Contains(textSearch)
                //|| s.Description.Contains(textSearch)
                )
                .AsNoTracking()
                .ToListAsync();
        }
        // Filter 
        // SOS

    }
}
