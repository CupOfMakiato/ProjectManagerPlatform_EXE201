using Server.Contracts.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentLinkAsync(CreatePaymentRequest createPaymentRequest);
    }
}
