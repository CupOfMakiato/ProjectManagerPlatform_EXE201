using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Repositories
{
    public interface IColumnRepository : IGenericRepository<Column>
    {
        Task<Column> GetColumnsById(Guid id);
        Task<List<Column>> GetListColumns();
    }
}
