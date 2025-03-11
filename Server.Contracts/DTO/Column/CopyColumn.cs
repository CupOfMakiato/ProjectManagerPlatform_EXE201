using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Column
{
    public class CopyColumn
    {
        public Guid ColumnId { get; set; }
        public string Title { get; set; }
    }
}
