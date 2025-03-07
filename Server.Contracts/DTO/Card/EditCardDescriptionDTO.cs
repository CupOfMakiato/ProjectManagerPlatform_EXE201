using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Card
{
    public class EditCardDescriptionDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
