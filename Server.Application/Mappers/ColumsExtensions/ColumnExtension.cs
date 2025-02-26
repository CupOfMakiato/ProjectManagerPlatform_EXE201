using Server.Application.Mappers.UserExtension;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using Server.Contracts.Abstractions.RequestAndResponse.Column;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Column;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Mappers.ColumsExtensions
{
    public static class ColumnExtention
    {
        public static ViewColumnDTO ToViewColumnDTO(this Column column)
        {
            return new ViewColumnDTO
            {
                Id = column.Id,
                Title = column.Title,
                Status = (Domain.Enums.ColumnStatus)column.Status,
                CreatedByUser = column.ColumnCreatedByUser.ToUserDTO()
            };
        }
        public static Column ToColums(this AddColumsDTO addColumsDTO)
        {
            return new Column
            {
                Id = addColumsDTO.Id,
                Title = addColumsDTO.Title,
                Status = addColumsDTO.Status,
                CreatedBy = addColumsDTO.UserId,
                BoardId = addColumsDTO.BoardId

            };
        }
        public static AddColumsDTO ToAddColumsDTO(this AddNewColumnRequest addNewColumnRequest)
        {
            return new AddColumsDTO
            {
                Id = (Guid)addNewColumnRequest.Id,
                UserId = addNewColumnRequest.UserId,
                Title = addNewColumnRequest.Title,
                BoardId = addNewColumnRequest.BoardId,
                Status = ColumnStatus.Open,

            };
        }
        public static UpdateBoardDTO ToUpdateBoardDTO(this UpdateBoardRequest updateBoardRequest)
        {
            return new UpdateBoardDTO
            {
                Id = (Guid)updateBoardRequest.Id,
                Title = updateBoardRequest.Title,
                Description = updateBoardRequest.Description,
            };
        }
    }
}
