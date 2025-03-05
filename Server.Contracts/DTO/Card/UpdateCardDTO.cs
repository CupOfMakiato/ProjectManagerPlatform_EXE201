using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Card
{
    public class UpdateCardDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CardStatus Status { get; set; }
        public AssignedCompletion AssignedCompletion { get; set; }
    }
}
