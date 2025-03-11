using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class Board : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public BoardType? Type { get; set; }
        public BoardStatus? Status { get; set; }
        public ICollection<Column> Columns { get; set; } = new List<Column>();
        public User BoardCreatedByUser { get; set; }
    }
}
