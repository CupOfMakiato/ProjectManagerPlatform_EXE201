﻿using Server.Domain.Enums;
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
        public string Description { get; set; }
        public string Attachment { get; set; }
        public string AttachmentId { get; set; }
        public CardStatus? Status { get; set; }
        public AssignedCompletion? AssignedCompletion { get; set; }

        [ForeignKey("ColumnId")]
        public Column Column { get; set; }
        public ICollection<Activity> CardActivities { get; set; } = new List<Activity>();
        public User CardCreatedByUser { get; set; }
    }
}
