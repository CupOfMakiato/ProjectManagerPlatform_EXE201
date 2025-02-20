using Server.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface IColumnService
    {
        Task<Result<object>> ViewAllColumns();
        Task<Result<object>> ViewColumnById(Guid columnId);
    }
}
