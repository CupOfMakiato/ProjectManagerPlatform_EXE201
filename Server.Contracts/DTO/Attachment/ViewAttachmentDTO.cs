using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Attachment
{
    public class ViewAttachmentDTO
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public string FilePublicId { get; set; }
        public bool IsCover { get; set; } = false;
    }
}
