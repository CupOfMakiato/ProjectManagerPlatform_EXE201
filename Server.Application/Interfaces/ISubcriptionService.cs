using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Subcription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface ISubcriptionService
    {
        Task<Result<object>> ViewAllSubcriptions();
        Task<Result<object>> AddNewSubcription(AddNewSubcriptionDTO AddNewSubcriptionDTO);
        Task<Result<object>> UpdateSubcription(UpdateSubcriptionDTO UpdateSubcriptionDTO);
        Task<Result<object>> DeleteSubcription(Guid subcriptionId);
    }
}
