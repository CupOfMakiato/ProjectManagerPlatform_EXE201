using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class Subcription : BaseEntity
    {
        public SubcriptionType SubcriptionName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Duration { get; set; }
        public User SubcriptionCreatedBy { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
    }
}
