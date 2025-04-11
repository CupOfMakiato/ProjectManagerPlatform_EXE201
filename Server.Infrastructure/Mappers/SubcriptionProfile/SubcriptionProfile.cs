using AutoMapper;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Subcription;
using Server.Contracts.DTO.User;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Mappers.SubcriptionProfile
{
    public class SubcriptionProfile : Profile
    {
        public SubcriptionProfile()
        {
            CreateMap<Subcription, ViewSubcriptionDTO>()
    .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src =>
        src.SubcriptionCreatedBy != null ? new UserDTO { Id = src.SubcriptionCreatedBy.Id, UserName = src.SubcriptionCreatedBy.UserName } : null));

        }
    }
}
