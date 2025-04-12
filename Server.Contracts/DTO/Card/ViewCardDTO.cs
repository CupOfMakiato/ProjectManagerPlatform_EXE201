using Server.Contracts.DTO.Attachment;
using Server.Contracts.DTO.Column;
using Server.Contracts.DTO.User;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Card
{
    public class ViewCardDTO
    {
        public Guid Id { get; set; }
        public ViewColumnDTO Column { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CardPosition { get; set; }
        public List<ViewAttachmentDTO>? Attachments { get; set; } = new List<ViewAttachmentDTO>();
        public CardStatus Status { get; set; }
        public AssignedCompletion AssignedCompletion { get; set; }
        public UserDTO? CreatedByUser { get; set; }
    }
}
