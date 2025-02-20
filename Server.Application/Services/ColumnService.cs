using AutoMapper;
using Microsoft.AspNetCore.Http;
using Server.Application.Interfaces;
using Server.Application.Mappers.CardExtension;
using Server.Application.Mappers.ColumnExtension;
using Server.Application.Repositories;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Card;
using Server.Contracts.DTO.Column;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Services
{
    public class ColumnService : IColumnService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IColumnRepository _columnRepository;
        public ColumnService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, IEmailService emailService, IUserService userService, IColumnRepository columnRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _emailService = emailService;
            _userService = userService;
            _columnRepository = columnRepository;
        }

        public async Task<Result<object>> ViewAllColumns()
        {
            var columns = await _unitOfWork.columnRepository.GetAllOpenColumns();

            var result = columns.Select(column => column.ToViewColumnDTO()).ToList();

            return new Result<object>
            {
                Error = result.Any() ? 0 : 1,
                Message = result.Any() ? "Columns retrieved successfully" : "No open columns found",
                Data = result
            };
        }

        public async Task<Result<object>> ViewColumnById(Guid columnId)
        {
            ViewColumnDTO result = null;
            var card = await _unitOfWork.columnRepository.GetColumnById(columnId);
            if (card != null)
                result = card.ToViewColumnDTO();
            return new Result<object>
            {
                Error = result != null ? 0 : 1,
                Message = result != null ? "Get column successfully" : "Get column fail",
                Data = result
            };
        }
    }
}
