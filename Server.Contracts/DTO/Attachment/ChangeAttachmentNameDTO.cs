using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Attachment
{
    public class ChangeAttachmentNameDTO
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
    }
}
