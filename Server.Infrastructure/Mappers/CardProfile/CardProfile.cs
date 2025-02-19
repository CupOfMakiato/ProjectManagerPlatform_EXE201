using AutoMapper;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Card;
using Server.Contracts.DTO.User;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Mappers.CardProfile
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, ViewCardDTO>()
    .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src =>
        src.CardCreatedByUser != null ? new UserDTO { Id = src.CardCreatedByUser.Id, UserName = src.CardCreatedByUser.UserName } : null));

        }
    }
}
