using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class CardActivity : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid CardId { get; set; }
        public string Action { get; set; }
        public string Detail { get; set; }
        public string Status { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; }
        public User User { get; set; }
    }
}
