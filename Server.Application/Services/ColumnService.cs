using AutoMapper;
using Microsoft.AspNetCore.Http;
using Server.Application.Common;
using Server.Application.Interfaces;
using Server.Application.Mappers.CardExtension;
using Server.Application.Mappers.ColumnExtension;
using Server.Application.Mappers.BoardExtension;
using Server.Application.Mappers.ColumsExtensions;
using Server.Application.Repositories;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Card;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Column;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Services
{
    public class ColumnService : IColumnService
    public class ColumnService : IColumnsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IColumnRepository _columnRepository;
        public ColumnService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, IEmailService emailService, IUserService userService, IColumnRepository columnRepository)
        private readonly IBoardService _boardService;

        public ColumnService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IUserService userService, IColumnRepository columnRepository, IBoardService boardService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _userService = userService;
            _columnRepository = columnRepository;
            _boardService = boardService;
        }
        public async Task<List<ViewColumnDTO>> ViewAllColumns()
        {
            var result = await _unitOfWork.columnRepository.GetListColumns();
            var mappedColumns = _mapper.Map<List<ViewColumnDTO>>(result);
            return mappedColumns;
        }

        public async Task<Result<object>> ViewAllColumns()
        public async Task<Result<object>> ViewColumnsById(Guid columnId)
        {
            var columns = await _unitOfWork.columnRepository.GetAllOpenColumns();
            ViewColumnDTO result = null;
            var column = await _unitOfWork.columnRepository.GetColumnsById(columnId);
            if (column != null)
                result = column.ToViewColumnDTO();
            return new Result<object>
            {
                Error = result != null ? 0 : 1,
                Message = result != null ? "Get column successfully" : "Get column fail",
                Data = result
            };
        }

        public async Task<Result<object>> AddNewColumn(AddColumsDTO addColumsDTO)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
                return new Result<object>() { Error = 1, Message = "Token not found", Data = null };

            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            var result = columns.Select(column => column.ToViewColumnDTO()).ToList();
            if (jwtToken == null)
                return new Result<object>() { Error = 1, Message = "Invalid token", Data = null };
            var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
            var getBoard = await _unitOfWork.boardRepository.GetByIdAsync(addColumsDTO.BoardId);
            if (getBoard == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Board does not exist!",
                    Data = null
                };
            }
            var columsMapper = addColumsDTO.ToColums(userId);
            
            await _unitOfWork.columnRepository.AddAsync(columsMapper);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result.Any() ? 0 : 1,
                Message = result.Any() ? "Columns retrieved successfully" : "No open columns found",
                Data = result
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Add new columm successfully" : "Add new columm fail",
                Data = null
            };
        }

        public async Task<Result<object>> ViewColumnById(Guid columnId)
        public Task<Result<object>> UpdateColumn(Guid id, UpdateBoardDTO updateBoardDTO)
        {
            ViewColumnDTO result = null;
            var card = await _unitOfWork.columnRepository.GetColumnById(columnId);
            if (card != null)
                result = card.ToViewColumnDTO();
            throw new NotImplementedException();
        }

        public async Task<Result<object>> DeleteColumn(Guid columnId)
        {
            var getColumn = await _unitOfWork.columnRepository.GetColumnsById(columnId);
            if (getColumn == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Columns not exist",
                    Data = null
                };
            }
            _unitOfWork.columnRepository.SoftRemove(getColumn);
            var result = await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = result != null ? 0 : 1,
                Message = result != null ? "Get column successfully" : "Get column fail",
                Data = result
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Column deleted successfully" : "Failed to delete column",
                Data = null
            };
        }
    }
}
