using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Card
{
    public class ChangeCardNameDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
