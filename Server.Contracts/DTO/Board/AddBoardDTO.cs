using Server.Contracts.DTO.User;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Board
{
    public class AddBoardDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public BoardType Type { get; set; } // Board, Calendar
        public BoardStatus Status { get; set; } // Closed, Opened
    }
}
