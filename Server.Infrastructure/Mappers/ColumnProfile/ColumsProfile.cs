using AutoMapper;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Column;
using Server.Contracts.DTO.User;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Mappers.ColumnProfile
{
    public class ColumsProfile : Profile
    { 
        public ColumsProfile()
        {
            CreateMap<Column, ViewColumnDTO>()
                .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src =>
        src.ColumnCreatedByUser != null ? new UserDTO { Id = src.ColumnCreatedByUser.Id, UserName = src.ColumnCreatedByUser.UserName } : null))
                .ForMember(dest => dest.Board, opt => opt.MapFrom(src =>
        src.ColumnCreatedByUser != null ? new ViewBoardDTO { Id = src.Board.Id } : null));
        }
    }
}
