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
        public string Description { get; set; }
        public string Cover { get; set; }
        public string CoverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CardStatus? Status { get; set; }

        [ForeignKey("ColumnId")]
        public Column Column { get; set; }
        public ICollection<CardActivity> CardActivities { get; set; } = new List<CardActivity>();
        public User CardCreatedByUser { get; set; }
    }
}
