using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class Payment : BaseEntity
    {
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public double TotalAmount { get; set; }
        [ForeignKey("SubcriptionId")]
        public Guid SubcriptionId { get; set; }

        public string PaymentUrl { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }

        public User User { get; set; }
        public Subcription Subcription { get; set; }
    }
}
