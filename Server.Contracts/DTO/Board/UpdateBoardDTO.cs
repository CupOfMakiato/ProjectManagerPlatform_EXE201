using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Board
{
    public class UpdateBoardDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public IFormFile? ThumbNail { get; set; } // add later
    }
}
