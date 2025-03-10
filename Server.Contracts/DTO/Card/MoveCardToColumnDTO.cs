using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Card
{
    public class MoveCardToColumnDTO
    {
        public Guid Id { get; set; }       
        public Guid NewColumnId { get; set; }  
        public int NewPosition { get; set; }   
    }

}
