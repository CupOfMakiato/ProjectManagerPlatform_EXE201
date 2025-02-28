using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.RequestAndResponse.Column
{
    public class AddNewColumnRequest
    {
        public Guid BoardId { get; set; }
        public string Title { get; set; }
        public ColumnStatus Status { get; set; }
    }
}
