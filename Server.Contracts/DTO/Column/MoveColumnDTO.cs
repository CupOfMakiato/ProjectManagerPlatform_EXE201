using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Column
{
    public class MoveColumnDTO
    {
        public Guid ColumnId { get; set; }
        public Guid BoardId { get; set; }
        public int ColumnPosition { get; set; }
    }
}
