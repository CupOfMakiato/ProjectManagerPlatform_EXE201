using Server.Application.Common;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface IBoardService
    {
        Task<Pagination<ViewBoardDTO>> ViewAllBoards(int pageIndex, int pageSize);
        Task<Pagination<ViewBoardDTO>> ViewAllClosedBoards(int pageIndex, int pageSize);
        Task<Result<object>> ViewBoardById(Guid serviceId);
        Task<Result<object>> AddNewBoard(AddBoardDTO addBoardDTO);
        Task<Result<object>> ArchiveBoard(Guid boardId);
        Task<Result<object>> UnarchiveBoard(Guid boardId);
        Task<Result<object>> UpdateBoard(UpdateBoardDTO updateBoardDTO);
        Task<Result<object>> ChangeBoardName(ChangeBoardNameDTO changeBoardNameDTO);
        Task<Result<object>> DeleteBoard(Guid boardId);

        // Filter 
    }
}
