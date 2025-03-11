using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.RequestAndResponse.Board
{
    public class AddNewBoardRequest
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public BoardStatus Status { get; set; }
        public BoardType Type { get; set; }
        //public IFormFile ThumbNail { get; set; }
    }
}
