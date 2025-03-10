using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.RequestAndResponse.Card
{
    public class DownloadAttachmentRequest
    {
        public Guid cardId { get; set; }
        public Guid attachmentId { get; set; }
    }
}
