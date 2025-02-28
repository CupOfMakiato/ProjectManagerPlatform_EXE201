using Server.Application.Mappers.UserExtension;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Column;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Mappers.ColumnExtension
{
    public static class ColumnExtensions
    {
        //public static ViewColumnDTO ToViewColumnDTO(this Column column)
        //{
        //    return new ViewColumnDTO
        //    {
        //        Id = column.Id,
        //        Title = column.Title,
        //        //CollumnPosition = column.CollumnPosition,
        //        Status = (Domain.Enums.ColumnStatus)column.Status,
        //        BoardId = column.BoardId,
        //        CreatedByUser = column.ColumnCreatedByUser.ToUserDTO()

        //    };
        //}
    }
}
