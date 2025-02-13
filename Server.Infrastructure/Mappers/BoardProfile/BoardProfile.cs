using AutoMapper;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.User;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Mappers.BoardProfile
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Board, ViewBoardDTO>()
    .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src =>
        src.BoardCreatedByUser != null ? new UserDTO { Id = src.BoardCreatedByUser.Id, UserName = src.BoardCreatedByUser.UserName } : null));

        }
    }
}
