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
        //Task<int> GetTotalColumnCount(ColumnStatus? status = null);
        //Task<List<Column>> GetAllOpenColumns();
        //Task<List<Column>> GetAllArchivedColumns();
        //Task<Column> GetColumnById(Guid id);
        Task<Column> GetColumnsById(Guid id);
        Task<List<Column>> GetListColumns();
        Task<List<Column>> GetListColumnByBoardId(Guid boardId);
        Task<Column> GetColumnByPositionAndBoardId(int position, Guid boardId);
        Task<List<Column>> GetArchivedColumnByBoardId(Guid boardId);
    }
}
