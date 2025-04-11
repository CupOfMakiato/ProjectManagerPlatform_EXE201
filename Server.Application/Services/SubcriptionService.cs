using AutoMapper;
using Microsoft.AspNetCore.Http;
using Server.Application.Interfaces;
using Server.Application.Mappers.SubcriptionExtension;
using Server.Application.Repositories;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Subcription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Services
{
    public class SubcriptionService : ISubcriptionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISubcriptionRepository _subcriptionRepository;
        public SubcriptionService(IUnitOfWork unitOfWork, IMapper mapper,
            IHttpContextAccessor contextAccessor, ISubcriptionRepository subcriptionRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _subcriptionRepository = subcriptionRepository;
        }

        public async Task<Result<object>> ViewAllSubcriptions()
        {
            var subcriptions = await _unitOfWork.subcriptionRepository.GetAllSubcriptions();
            var result = subcriptions.Select(subcription => subcription.ToViewSubcriptionDTO()).ToList();
            return new Result<object>
            {
                Error = result.Any() ? 0 : 1,
                Message = result.Any() ? "Subcriptions retrieved successfully" : "No Subcriptions found",
                Data = result
            };
        }
        public async Task<Result<object>> AddNewSubcription(AddNewSubcriptionDTO AddNewSubcriptionDTO)
        {
            var user = await _unitOfWork.userRepository.GetByIdAsync(AddNewSubcriptionDTO.UserId);
            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "User does not exist!",
                    Data = null
                };
            }
            var subcriptionMapper = AddNewSubcriptionDTO.ToSubcription();
            
            await _unitOfWork.subcriptionRepository.AddAsync(subcriptionMapper);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Add new subcription successfully" : "Add new subcription fail",
                Data = null
            };
        }
        public async Task<Result<object>> UpdateSubcription(UpdateSubcriptionDTO UpdateSubcriptionDTO)
        {
            var subcription = await _unitOfWork.subcriptionRepository.GetSubcriptionById(UpdateSubcriptionDTO.Id);
            if (subcription == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Subcription not found",
                    Data = null
                };
            }

            subcription.SubcriptionName = UpdateSubcriptionDTO.SubcriptionName;
            subcription.Description = UpdateSubcriptionDTO.Description;
            subcription.Price = UpdateSubcriptionDTO.Price;
            

            _unitOfWork.subcriptionRepository.Update(subcription);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Update subcription successfully" : "Update subcription failed",
                Data = null
            };
        }

        public async Task<Result<object>> DeleteSubcription(Guid subcriptionId)
        {
            // Retrieve the existing subcription
            var existingSubcription = await _unitOfWork.subcriptionRepository.GetByIdAsync(subcriptionId);
            if (existingSubcription == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Subcription not found",
                    Data = null
                };
            }

            // Soft delete the subcription
            _unitOfWork.subcriptionRepository.SoftRemove(existingSubcription);

            // Save the changes
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Subcription deleted successfully" : "Failed to delete subcription",
                Data = null
            };
        }
    }
}
