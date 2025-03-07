using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.RequestAndResponse.Attachment
{
    public class ChangeAttachmentNameRequest
    {
        public Guid attachmentId { get; set; }
        public string FileName { get; set; }
    }
}
