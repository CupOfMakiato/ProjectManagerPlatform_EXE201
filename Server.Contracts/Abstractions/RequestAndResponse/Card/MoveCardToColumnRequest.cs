using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.RequestAndResponse.Card
{
    public class MoveCardToColumnRequest
    {
        public Guid Id { get; set; } 
        public Guid NewColumnId { get; set; } 
        public int NewPosition { get; set; } 
    }
}
