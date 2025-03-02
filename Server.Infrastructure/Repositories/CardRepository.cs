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
            return await _dbContext.Cards
                .Include(c => c.Column) // Include Column to access BoardId
                .ThenInclude(col => col.Board) // Include Board for verification
                .Include(c => c.Attachments)
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Include(s => s.CardCreatedByUser).ToListAsync();
        }

        public async Task<List<Card>> GetAllOpenCards()
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include Column to access BoardId
                .ThenInclude(col => col.Board) // Include Board for verification
                .Include(c => c.Attachments)
                .Where(c => c.Status == CardStatus.Open)
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Include(s => s.CardCreatedByUser).ToListAsync();
        }

        public async Task<List<Card>> GetAllArchivedCards()
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include Column to access BoardId
                .ThenInclude(col => col.Board) // Include Board for verification
                .Include(c => c.Attachments)
                .Where(c => c.Status == CardStatus.Closed)
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Include(s => s.CardCreatedByUser).ToListAsync();
        }

        public async Task<List<Card>> GetCardsByBoardId(Guid boardId)
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include Column to access BoardId
                .ThenInclude(col => col.Board) // Include Board for verification
                .Include(c => c.Attachments)
                .Where(c => !c.IsDeleted && c.Column.BoardId == boardId) // Filter by boardId
                .OrderByDescending(c => c.ModificationDate) // Sort by last update
                .Include(s => s.CardCreatedByUser)
                .ToListAsync();
        }


        public async Task<Card?> GetCardById(Guid id)
        {
            return await _dbContext.Cards
                .Include(c => c.Column) 
                .ThenInclude(col => col.Board) 
                .Include(c => c.Attachments) 
                .Include(c => c.CardCreatedByUser) 
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted); 
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
