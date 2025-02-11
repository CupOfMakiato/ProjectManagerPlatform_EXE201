using AutoMapper;
using Server.Contracts.DTO.User;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Mappers.UserProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => 0));
        }
    }
}
