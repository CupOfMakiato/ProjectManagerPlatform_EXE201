using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Repositories
{
    public interface ISubcribeRepository : IGenericRepository<Subcribe>
    {
        Task<List<Subcribe>> GetSubcribesAsync();
        Task<Subcribe> GetSubcribeById(Guid id);
        Task AddSubcribeAsync(Subcribe subcribe);
        Task<Subcribe> CheckExist(Guid subcriptionId, Guid userId);
    }
}
