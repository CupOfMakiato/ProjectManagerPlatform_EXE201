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
    public class ColumsRepository : GenericRepository<Column>, IColumnRepository
    {
        private readonly AppDbContext _dbContext;

        public ColumsRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<Column> GetColumnsById(Guid id)
        {
            var board = await _dbContext.Columns
                .Where(c => c.Id == id)
                .Include(c => c.Board)
                .Include(c => c.ColumnCreatedByUser)
                .Include(c => c.Cards)
                .FirstOrDefaultAsync();
            return board;
        }

        public async Task<List<Column>> GetListColumnByBoardId(Guid boardId)
        {
            return await _dbContext.Columns
                .Include(c => c.Board)
                .Include(c => c.ColumnCreatedByUser)
                .Where(c => !c.IsDeleted)
                .Where(c => c.Status == ColumnStatus.Open)
                .Where(c => c.BoardId == boardId)
                .OrderBy(c => c.CollumnPosition)
                .ToListAsync();
        }

        public async Task<List<Column>> GetArchivedColumnByBoardId(Guid boardId)
        {
            return await _dbContext.Columns
                .Include(c => c.Board)
                .Include(c => c.ColumnCreatedByUser)
                .Where(c => !c.IsDeleted)
                .Where(c => c.Status == ColumnStatus.Closed)
                .Where(c => c.BoardId == boardId)
                .OrderBy(c => c.CollumnPosition)
                .ToListAsync();
        }

        public async Task<List<Column>> GetListColumns()
        {
            var listColumns = await _dbContext.Columns
                .Include(c => c.Board)
                .Include(c => c.ColumnCreatedByUser)
                .OrderBy(c => c.BoardId)
                .ToListAsync();

            return listColumns;
        }
        public async Task<Column> GetColumnByPositionAndBoardId(int position, Guid boardId)
        {
            return await _dbContext.Columns.FirstOrDefaultAsync(c => c.CollumnPosition == position && c.BoardId == boardId);
        }
    }
}
