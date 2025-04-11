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
        Task<List<Board>> GetBoardsAsync();
        Task<List<Board>> GetAllOpenBoards(Guid userId);
        Task<List<Board>> GetAllClosedBoards(Guid userId);
        Task<int> GetTotalBoardCount(BoardStatus? status = null);
        Task<Board> GetBoardById(Guid id);
        Task<List<Board>> GetPagedBoards(int pageIndex, int pageSize, BoardStatus? status = null);
        Task<List<Board>> SearchBoardsAsync(string textSearch, Guid userId);
        Task<List<Board>> GetAllClosedBoardsCreatedByUser(Guid userId);
        Task<List<Board>> GetAllOpenBoardsCreatedByUser(Guid userId);
        Task<List<Board>> GetPagedBoardsCreatedByUser(Guid userId, int pageIndex, int pageSize, BoardStatus? status = null);
    }
}
