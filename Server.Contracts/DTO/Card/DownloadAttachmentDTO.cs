using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Card
{
    public class DownloadAttachmentDTO
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
