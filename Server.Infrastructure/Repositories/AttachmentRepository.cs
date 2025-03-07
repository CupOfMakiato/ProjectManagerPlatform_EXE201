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
    public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        private readonly AppDbContext _dbContext;

        public AttachmentRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Attachment>> GetAllAttachments()
        {
            return await _dbContext.Attachments.ToListAsync();
        }

        public async Task<List<Attachment>> GetAttachmentsByCardId(Guid cardId)
        {
            return await _dbContext.Attachments.Where(a => a.CardId == cardId).ToListAsync();
        }

        public async Task<Attachment> GetAttachmentById(Guid id)
        {
            return await _dbContext.Attachments.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalAttachmentCount()
        {
            return await _dbContext.Attachments.Where(a => !a.IsDeleted).CountAsync();
        }

        public async Task<Attachment> GetCoverByCardId(Guid cardId)
        {
            return await _dbContext.Attachments
                .FirstOrDefaultAsync(a => a.CardId == cardId && a.IsCover);
        }


    }
}
