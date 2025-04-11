using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Payment
{
    public class CreatePaymentRequest
    {
        public Guid SubcriptionId { get; set; }
    }
}
