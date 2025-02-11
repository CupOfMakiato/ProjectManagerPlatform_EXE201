using Microsoft.EntityFrameworkCore;
using Server.Application.Common;
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
    public class BoardRepository :GenericRepository<Board>, IBoardRepository
    {
        private readonly AppDbContext _dbContext;

        public BoardRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetTotalBoardCount(BoardStatus? status = null)
        {
            var query = _dbContext.Boards.Where(c => !c.IsDeleted);

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            return await query.CountAsync();
        }

        public async Task<Board> GetBoardById(Guid id)
        {
            return await _dbContext.Boards.Where(c => c.Id == id).Include(c => c.CreatedByUser).FirstOrDefaultAsync();
        }
        public async Task<List<Board>> GetPagedBoards(int pageIndex, int pageSize, BoardStatus? status = null)
        {
            var query = _dbContext.Boards
                .Where(c => !c.IsDeleted);

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            return await query
                .OrderByDescending(c => c.UpdatedAt)  // Sorting by last updated
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(s => s.CreatedByUser)
                .ToListAsync();
        }

        public async Task<List<Board>> SearchBoardsAsync(string textSearch)
        {
            return await _dbContext.Boards
                .Where(s => s.Title.Contains(textSearch) || s.Description.Contains(textSearch))
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
