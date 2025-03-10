using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.RequestAndResponse.Card
{
    public class ChangeCardNameRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
