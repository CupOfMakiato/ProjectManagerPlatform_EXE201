using AutoMapper;
using Server.Contracts.DTO.SubCategory;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Mappers.SubCategoryProfile
{
    public class SubCategoryProfile : Profile
    {
        public SubCategoryProfile()
        {
            CreateMap<ViewSubCategoryDTO, SubCategory>().ReverseMap()
               .ForMember(p => p.SubId, b => b.MapFrom(m => m.Id));
            CreateMap<CreateSubCategoryDTO, SubCategory>().ReverseMap();
            CreateMap<SubCategory, UpdateSubCategoryDTO>().ReverseMap();
        }
    }
}
