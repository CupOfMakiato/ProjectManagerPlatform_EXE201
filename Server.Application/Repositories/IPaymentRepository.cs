using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task AddPaymentAsync(Payment payment);
    }
}
