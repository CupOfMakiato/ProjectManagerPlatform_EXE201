using AutoMapper;
using Server.Contracts.DTO.Attachment;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Card;
using Server.Contracts.DTO.Column;
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
                            src.CardCreatedByUser != null ? new UserDTO 
                            { Id = src.CardCreatedByUser.Id, UserName = src.CardCreatedByUser.UserName } : null))
                .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.Column)) // Map single Column
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments)); // Map Attachments

            CreateMap<Attachment, ViewAttachmentDTO>(); // Ensure Attachment mapping exists
            CreateMap<Column, ViewColumnDTO>(); // Ensure Column mapping exists

        }
    }
}
