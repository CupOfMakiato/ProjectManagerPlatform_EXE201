using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class Card : BaseEntity
    {
        public Guid ColumnId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CardPosition { get; set; }
        public CardStatus? Status { get; set; }
        public AssignedCompletion? AssignedCompletion { get; set; }

        [ForeignKey("ColumnId")]
        public Column Column { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        // List of attachments
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public User CardCreatedByUser { get; set; }

        public DateTime? DueDate { get; set; } 
        public DateTime? StartDate { get; set; }
        public ReminderType? Reminder { get; set; } = ReminderType.None;
    }
}
