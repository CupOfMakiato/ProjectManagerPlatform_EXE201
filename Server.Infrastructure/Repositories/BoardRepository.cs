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
    public class BoardRepository : GenericRepository<Board>, IBoardRepository
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

        public async Task<List<Board>> GetBoardsAsync()
        {
            return await _dbContext.Boards
                .Include(c => c.BoardCreatedByUser)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Board>> GetAllOpenBoards(Guid userId)
        {
            return await _dbContext.Boards
                .Where(c => c.Status == BoardStatus.Open)
                .Where(b => b.CreatedBy == userId)
                .Where(c => !c.IsDeleted)
                .Include(c => c.BoardCreatedByUser)
                .ToListAsync();
        }


        public async Task<List<Board>> GetAllClosedBoards(Guid userId)
        {
            return await _dbContext.Boards
                .Where(c => c.Status == BoardStatus.Closed)
                .Where(b => b.CreatedBy == userId)
                .Where(c => !c.IsDeleted)
                .Include(c => c.BoardCreatedByUser)
                .ToListAsync();
        }
        public async Task<List<Board>> GetAllOpenBoardsCreatedByUser(Guid userId)
        {
            return await _dbContext.Boards
                .Where(b => b.Status == BoardStatus.Open)       
                .Where(b => !b.IsDeleted)                       
                .Where(b => b.CreatedBy == userId)              
                .Include(b => b.BoardCreatedByUser)             
                .ToListAsync();
        }

        public async Task<List<Board>> GetAllClosedBoardsCreatedByUser(Guid userId)
        {
            return await _dbContext.Boards
                .Where(b => b.Status == BoardStatus.Closed)
                .Where(b => !b.IsDeleted)
                .Where(b => b.CreatedBy == userId)
                .Include(b => b.BoardCreatedByUser)
                .ToListAsync();
        }

        public async Task<Board> GetBoardById(Guid id)
        {
            return await _dbContext.Boards.Where(c => c.Id == id).Include(c => c.BoardCreatedByUser).FirstOrDefaultAsync();
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
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(s => s.BoardCreatedByUser)
                .ToListAsync();
        }

        public async Task<List<Board>> GetPagedBoardsCreatedByUser(Guid userId, int pageIndex, int pageSize, BoardStatus? status = null)
        {
            var query = _dbContext.Boards
                .Where(b => b.CreatedBy == userId)
                .Where(c => !c.IsDeleted);

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            return await query
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(s => s.BoardCreatedByUser)
                .ToListAsync();
        }

        public async Task<List<Board>> SearchBoardsAsync(string textSearch, Guid userId)
        {
            return await _dbContext.Boards
                .Where(s => s.Title.Contains(textSearch)
                //|| s.Description.Contains(textSearch)
                )
                .Where(b => b.CreatedBy == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Filter 


    }
}
