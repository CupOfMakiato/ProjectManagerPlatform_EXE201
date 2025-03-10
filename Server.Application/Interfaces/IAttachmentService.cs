using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Attachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface IAttachmentService
    {
        // update thing
        Task<Result<object>> ChangeAttachmentName(ChangeAttachmentNameDTO changeAttachmentNameDTO);
        Task<Result<object>> MakeCover(Guid attachmentId);
        Task<Result<object>> RemoveCover(Guid attachmentId);
    }
}
