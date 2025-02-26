using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Column
{
    public class AddColumsDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid BoardId { get; set; }
        public Guid UserId { get; set; }
        public BoardType Type { get; set; } // Board, Calendar
        public ColumnStatus Status { get; set; } // Closed, Opened
    }
}
