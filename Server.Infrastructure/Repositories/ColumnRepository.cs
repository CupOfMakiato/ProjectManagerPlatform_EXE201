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

//namespace Server.Infrastructure.Repositories
//{
    //public class ColumnRepository : GenericRepository<Column>, IColumnRepository
    //{
        //    private readonly AppDbContext _dbContext;

        //    public ColumnRepository(AppDbContext dbContext,
        //        ICurrentTime timeService,
        //        IClaimsService claimsService)
        //        : base(dbContext,
        //              timeService,
        //              claimsService)
        //    {
        //        _dbContext = dbContext;
        //    }

        //    public async Task<int> GetTotalColumnCount(ColumnStatus? status = null)
        //    {
        //        var query = _dbContext.Columns.Where(c => !c.IsDeleted);

        //        if (status.HasValue)
        //        {
        //            query = query.Where(c => c.Status == status.Value);
        //        }

        //        return await query.CountAsync();
        //    }

        //    public async Task<List<Column>> GetAllOpenColumns()
        //    {
        //        return await _dbContext.Columns.Where(c => c.Status != ColumnStatus.Open).ToListAsync();
        //    }

        //    public async Task<List<Column>> GetAllArchivedColumns()
        //    {
        //        return await _dbContext.Columns.Where(c => c.Status != ColumnStatus.Closed).ToListAsync();
        //    }

        //    public async Task<Column> GetColumnById(Guid id)
        //    {
        //        return await _dbContext.Columns.Where(c => c.Id == id)
        //            .Include(c => c.ColumnCreatedByUser).FirstOrDefaultAsync();
        //    }
    //}
//}
