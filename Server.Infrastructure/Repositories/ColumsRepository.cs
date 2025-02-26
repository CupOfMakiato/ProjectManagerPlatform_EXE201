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
                .Include(c => c.Board)
                .Include(c => c.ColumnCreatedByUser)
                .FirstOrDefaultAsync(c => c.Id == id);
            return board;
        }

        public async Task<List<Column>> GetListColumns()
        {
            var listColumns = await _dbContext.Columns
                .Include(c => c.Board)
                .Include(c => c.ColumnCreatedByUser)
                .ToListAsync();

            return listColumns;
        }
    }
}
