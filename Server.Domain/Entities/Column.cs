using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class Column : BaseEntity
    {
        public string Title { get; set; }
        public int CollumnPosition { get; set; }
        public ColumnStatus Status { get; set; }
        [ForeignKey("BoardId")]
        public Guid BoardId { get; set; }
        public Board Board { get; set; }
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public User ColumnCreatedByUser { get; set; }
    }
}
