using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Repositories
{
    public interface IBoardRepository : IGenericRepository<Board>
    {
        Task<int> GetTotalBoardCount(BoardStatus? status = null);
        Task<Board> GetBoardById(Guid id);
        Task<List<Board>> GetPagedBoards(int pageIndex, int pageSize, BoardStatus? status = null);
        Task<List<Board>> SearchBoardsAsync(string textSearch);
    }
}
