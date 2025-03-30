using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class Subcribe : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [ForeignKey("SubcriptionId")]
        public Guid SubcriptionId { get; set; }
        public Subcription Subcription { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public User UserSubcribe { get; set; }
    }
}
