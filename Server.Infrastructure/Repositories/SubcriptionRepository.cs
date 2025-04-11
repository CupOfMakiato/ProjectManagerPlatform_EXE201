using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Repositories
{
    public class SubcriptionRepository : GenericRepository<Subcription>, ISubcriptionRepository
    {
        private readonly AppDbContext _dbContext;

        public SubcriptionRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Subcription>> GetAllSubcriptions()
        {
            return await _dbContext.Subcriptions
                .Include(c => c.SubcriptionCreatedBy)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }
        public async Task<Subcription> GetSubcriptionById(Guid id)
        {
            return await _dbContext.Subcriptions
                .Include(c => c.SubcriptionCreatedBy)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }
    }
}
