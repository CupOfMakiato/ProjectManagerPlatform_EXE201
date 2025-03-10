using Server.Application.Common;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Column;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface IColumnsService
    {
        Task<List<ViewColumnDTO>> ViewAllColumns();
        Task<Result<object>> ViewColumnsById(Guid columnId);
        Task<List<ViewColumnDTO>> ViewColumnsByBoardId(Guid boardId);
        Task<Result<object>> AddNewColumn(AddColumsDTO addColumsDTO);
        Task<Result<object>> UpdateColumn(Guid id, UpdateBoardDTO updateBoardDTO);
        Task<Result<object>> DeleteColumn(Guid columnId);
        Task<Result<object>> MoveColumnInBoard(MoveColumnDTO moveColumnDTO);
    }
}
