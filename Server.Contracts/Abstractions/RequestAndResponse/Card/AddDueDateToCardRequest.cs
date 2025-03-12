using Newtonsoft.Json;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.RequestAndResponse.Card
{
    public class AddDueDateToCardRequest
    {
        public Guid CardId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public ReminderType Reminder { get; set; } = ReminderType.None;
    }
}
