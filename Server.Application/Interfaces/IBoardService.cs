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
        // View 
        Task<Pagination<ViewBoardDTO>> ViewAllBoardsPagin(Guid userId, int pageIndex, int pageSize);
        Task<Pagination<ViewBoardDTO>> ViewAllClosedBoardsPagin(Guid userId, int pageIndex, int pageSize);
        Task<Result<object>> ViewBoardById(Guid serviceId);
        Task<Result<object>> ViewAllOpenBoards(Guid userId);
        Task<Result<object>> ViewAllClosedBoards(Guid userId);


        // Add
        Task<Result<object>> AddNewBoard(AddBoardDTO addBoardDTO);
        // update
        Task<Result<object>> ArchiveBoard(Guid boardId);
        Task<Result<object>> UnarchiveBoard(Guid boardId);
        Task<Result<object>> UpdateBoard(UpdateBoardDTO updateBoardDTO);
        Task<Result<object>> ChangeBoardName(ChangeBoardNameDTO changeBoardNameDTO);
        // Delete
        Task<Result<object>> DeleteBoard(Guid boardId);
        //view cards
        Task<Result<object>> ViewAllCardsFromABoard(Guid boardId);

        // Filter 
    }
}
