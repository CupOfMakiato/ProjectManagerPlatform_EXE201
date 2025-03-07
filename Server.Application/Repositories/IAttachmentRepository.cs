using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Repositories
{
    public interface IAttachmentRepository : IGenericRepository<Attachment>
    {
        Task<List<Attachment>> GetAllAttachments();
        Task<List<Attachment>> GetAttachmentsByCardId(Guid cardId);
        Task<Attachment> GetAttachmentById(Guid id);
        Task<int> GetTotalAttachmentCount();
    }
}
