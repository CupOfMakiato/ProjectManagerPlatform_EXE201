using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class Board : BaseEntity
    {
        public int ColumnOrder { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BoardType? Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public BoardStatus? Status { get; set; }
        public ICollection<Column> Columns { get; set; } = new List<Column>();
        public Guid CreatedBy { get; set; }
        public User CreatedByUser { get; set; }
    }
}
