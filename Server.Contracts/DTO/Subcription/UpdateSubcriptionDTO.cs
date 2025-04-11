using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Subcription
{
    public class UpdateSubcriptionDTO
    {
        public Guid Id { get; set; }
        public SubcriptionType SubcriptionName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}
