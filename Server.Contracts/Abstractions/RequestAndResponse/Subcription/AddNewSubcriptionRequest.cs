using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.RequestAndResponse.Subcription
{
    public class AddNewSubcriptionRequest
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public SubcriptionType SubcriptionName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Duration { get; set; }
        
    }
}
