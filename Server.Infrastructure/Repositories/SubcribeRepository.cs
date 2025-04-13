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
    public class SubcribeRepository : GenericRepository<Subcribe>, ISubcribeRepository
    {
        private readonly AppDbContext _dbContext;
        public SubcribeRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task AddSubcribeAsync(Subcribe subcribe)
        {
            await _dbContext.Subcribes.AddAsync(subcribe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Subcribe> CheckExist(Guid subcriptionId, Guid userId)
        {
            return await _dbContext.Subcribes.FirstOrDefaultAsync(s => s.SubcriptionId == subcriptionId && s.UserId == userId);
        }

        public async Task<Subcribe> GetSubcribeById(Guid id)
        {
            return await _dbContext.Subcribes
                .Include(c => c.UserId)
                .Include(c => c.SubcriptionId)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Subcribe>> GetSubcribesAsync()
        {
            return await _dbContext.Subcribes
                .Include(c => c.UserId)
                .Include(c => c.SubcriptionId)
                .ToListAsync();
        }
    }
}
