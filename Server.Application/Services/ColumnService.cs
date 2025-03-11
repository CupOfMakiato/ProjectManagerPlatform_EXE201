using AutoMapper;
using Microsoft.AspNetCore.Http;
using Server.Application.Common;
using Server.Application.Interfaces;
using Server.Application.Mappers.CardExtension;
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
using Server.Domain.Entities;

namespace Server.Application.Services
{
    public class ColumnService : IColumnsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IColumnRepository _columnRepository;
        private readonly IBoardService _boardService;

        public ColumnService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IUserService userService, IColumnRepository columnRepository, IBoardService boardService)
        {
            _mapper = mapper;
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

        public async Task<Result<object>> ViewColumnsById(Guid columnId)
        {
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

        public async Task<List<ViewColumnDTO>> ViewColumnsByBoardId(Guid boardId)
        {
            var getColumns = await _unitOfWork.columnRepository.GetListColumnByBoardId(boardId);
            var mappedColumns = _mapper.Map<List<ViewColumnDTO>>(getColumns);
            return mappedColumns;
        }

        public async Task<Result<object>> AddNewColumn(AddColumsDTO addColumsDTO)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
                return new Result<object>() { Error = 1, Message = "Token not found", Data = null };

            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return new Result<object>() { Error = 1, Message = "Invalid token", Data = null };
            var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
            var getBoard = await _unitOfWork.boardRepository.GetByIdAsync(addColumsDTO.BoardId);
            var getAllColumn = await _columnRepository.GetListColumnByBoardId(addColumsDTO.BoardId);
            var count = getAllColumn.Count;

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
            columsMapper.CollumnPosition = ++count;

            await _unitOfWork.columnRepository.AddAsync(columsMapper);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Add new columm successfully" : "Add new columm fail",
                Data = null
            };
        }

        public Task<Result<object>> UpdateColumn(Guid id, UpdateBoardDTO updateBoardDTO)
        {
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
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Column deleted successfully" : "Failed to delete column",
                Data = null
            };
        }

        public async Task<Result<object>> MoveColumnInBoard(MoveColumnDTO moveColumnDTO)
        {
            var getAllColumn = await _columnRepository.GetListColumns();
            var getColumn = await _columnRepository.GetColumnsById(moveColumnDTO.ColumnId);
            var getColumnMoveTo = await _columnRepository.GetColumnByPositionAndBoardId(moveColumnDTO.ColumnPosition, moveColumnDTO.BoardId);
            var currentPosition = getColumn.CollumnPosition;
            if(getColumn == null)
                return new Result<object>
                {
                    Error = 1,
                    Message = "Column not found",
                    Data = null
                };
            if(getColumn.BoardId != moveColumnDTO.BoardId)
                return new Result<object>
                {
                    Error = 1,
                    Message = "Column does not belong to the this Board",
                    Data = null
                };
            var getListColumnByBoardId = await _columnRepository.GetListColumnByBoardId(getColumn.BoardId);
            var count = getListColumnByBoardId.Count;

            if(moveColumnDTO.ColumnPosition < 1 || moveColumnDTO.ColumnPosition > count)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = $"Position of Column must be greater than 0 or less than or equal to {count} ",
                    Data = null
                };
            }
            else
            {
                if (moveColumnDTO.ColumnPosition == getColumn.CollumnPosition)
                {
                    return new Result<object>
                    {
                        Error = 0,
                        Message = "Column position not change",
                        Data = null
                    };
                }
                getColumn.CollumnPosition = moveColumnDTO.ColumnPosition;   
                getColumnMoveTo.CollumnPosition = currentPosition;
                _unitOfWork.columnRepository.Update(getColumn);
                _unitOfWork.columnRepository.Update(getColumnMoveTo);
                var result = await _unitOfWork.SaveChangeAsync();
                return new Result<object>
                {
                    Error = 0,
                    Message = "Column position was changed",
                    Data = null
                };
            }
        }

        public async Task<Result<object>> CopyColumn(CopyColumn copyColumn)
        {
            var getColumn = await _columnRepository.GetColumnsById(copyColumn.ColumnId);
            var getListColumn = await _columnRepository.GetListColumnByBoardId(getColumn.BoardId);
            int count = getListColumn.Count;
            
            if(getColumn == null)
                return new Result<object>
                {
                    Error = 1,
                    Message = "Column not found",
                    Data = null
                };

            var newColumn = new Column()
            {
                Id = Guid.NewGuid(),
                Title = copyColumn.Title,   
                CollumnPosition = count++,
                Status = ColumnStatus.Open,
                BoardId = getColumn.BoardId,
                CreationDate = DateTime.Now,
                CreatedBy = getColumn.CreatedBy,
                IsDeleted = false,
                Cards = new List<Card>()
            };

            await _unitOfWork.columnRepository.AddAsync(newColumn);

            foreach (var oldCard in getColumn.Cards)
            {
                var newCard = new Card
                {
                    Id = Guid.NewGuid(), // Tạo Id mới cho Card
                    Title = oldCard.Title,
                    Description = oldCard.Description,
                    ColumnId = newColumn.Id, // Gán vào Column mới
                    CardPosition = oldCard.CardPosition, // Giữ nguyên vị trí
                    Status = oldCard.Status,
                    AssignedCompletion = oldCard.AssignedCompletion,
                    IsDeleted = false // Đảm bảo không bị xóa
                };

                await _unitOfWork.cardRepository.AddAsync(newCard); // Thêm Card mới vào Column mới
                _unitOfWork.SaveChangeAsync();
            }
            _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = 0,
                Message = "Copy Column Successfully",
                Data = null
            };
        }
    }
}
