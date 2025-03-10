using Server.Contracts.DTO.Attachment;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Mappers.AttachmentExtension
{
    public static class AttachmentExtensions
    {
        public static ViewAttachmentDTO ToViewAttachmentDTO(this Attachment attachment)
        {
            return new ViewAttachmentDTO
            {
                Id = attachment.Id,
                FileName = attachment.FileName,
                FileUrl = attachment.FileUrl,
                FileType = attachment.FileType,
                IsCover = attachment.IsCover,
            };
        }
    }
}
