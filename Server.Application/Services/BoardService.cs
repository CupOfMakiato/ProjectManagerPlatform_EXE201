﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Server.Application.Common;
using Server.Application.Interfaces;
using Server.Application.Mappers.BoardExtension;
using Server.Application.Mappers.CardExtension;
using Server.Application.Repositories;
using Server.Contracts.Abstractions.CloudinaryService;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Card;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Services
{
    public class BoardService : IBoardService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IBoardRepository _boardRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IRedisService _redisService;
        public BoardService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService,
            IRedisService redisService,
            IHttpContextAccessor contextAccessor, IEmailService emailService, IUserService userService,
            IBoardRepository boardRepository, ICardRepository cardRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _emailService = emailService;
            _userService = userService;
            _boardRepository = boardRepository;
            _cardRepository = cardRepository;
            _redisService = redisService;
        }
        //public async Task<Pagination<ViewBoardDTO>> ViewAllBoardsPagin(int pageIndex, int pageSize)
        //{
        //    string cacheKey = $"boards_{pageIndex}_{pageSize}"; // Unique cache key

        //    // Check if data exists in Redis cache
        //    var cachedData = await _redisService.GetAsync<Pagination<ViewBoardDTO>>(cacheKey);
        //    if (cachedData is not null)
        //    {
        //        return cachedData; // Return cached data
        //    }

        //    // Fetch data from database
        //    var totalItemsCount = await _unitOfWork.boardRepository.GetTotalBoardCount(BoardStatus.Open);
        //    var services = await _unitOfWork.boardRepository.GetPagedBoards(pageIndex, pageSize, BoardStatus.Open);
        //    var mappedBoards = _mapper.Map<List<ViewBoardDTO>>(services);

        //    var result = new Pagination<ViewBoardDTO>
        //    {
        //        TotalItemsCount = totalItemsCount,
        //        PageSize = pageSize,
        //        PageIndex = pageIndex,
        //        Items = mappedBoards
        //    };

        //    // Store in Redis cache for 10 minutes
        //    await _redisService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

        //    return result;
        //}

        // Remove Redis caching
        public async Task<Pagination<ViewBoardDTO>> ViewAllBoardsPagin(Guid userId, int pageIndex, int pageSize)
        {
            var totalItemsCount = await _unitOfWork.boardRepository.GetTotalBoardCount(BoardStatus.Open);
            var services = await _unitOfWork.boardRepository.GetPagedBoardsCreatedByUser(userId, pageIndex, pageSize, BoardStatus.Open);
            var mappedBoards = _mapper.Map<List<ViewBoardDTO>>(services);

            return new Pagination<ViewBoardDTO>
            {
                TotalItemsCount = totalItemsCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = mappedBoards
            };
        }


        public async Task<Pagination<ViewBoardDTO>> ViewAllClosedBoardsPagin(Guid userId, int pageIndex, int pageSize)
        {
            var totalItemsCount = await _unitOfWork.boardRepository.GetTotalBoardCount(BoardStatus.Closed);
            var closedBoards = await _unitOfWork.boardRepository.GetPagedBoardsCreatedByUser(userId, pageIndex, pageSize, BoardStatus.Closed);
            var mappedBoards = _mapper.Map<List<ViewBoardDTO>>(closedBoards);

            return new Pagination<ViewBoardDTO>
            {
                TotalItemsCount = totalItemsCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = mappedBoards
            };
        }

        public async Task<Result<object>> ViewAllOpenBoards(Guid userId)
        {
            var boards = await _unitOfWork.boardRepository.GetAllOpenBoards(userId);
            var result = boards.Select(board => board.ToViewBoardDTO()).ToList();
            return new Result<object>
            {
                Error = result.Any() ? 0 : 1,
                Message = result.Any() ? "Opened boards retrieved successfully" : "No open boards found",
                Data = result
            };
        }

        public async Task<Result<object>> ViewAllClosedBoards(Guid userId)
        {
            var boards = await _unitOfWork.boardRepository.GetAllClosedBoards(userId);
            var result = boards.Select(board => board.ToViewBoardDTO()).ToList();
            return new Result<object>
            {
                Error = result.Any() ? 0 : 1,
                Message = result.Any() ? "Closed boards retrieved successfully" : "No closed boards found",
                Data = result
            };
        }

        public async Task<Result<object>> ViewBoardById(Guid cardId)
        {
            ViewBoardDTO result = null;
            var board = await _unitOfWork.boardRepository.GetBoardById(cardId);
            if (board != null)
                result = board.ToViewBoardDTO();
            return new Result<object>
            {
                Error = result != null ? 0 : 1,
                Message = result != null ? "Get board successfully" : "Get board fail",
                Data = result
            };
        }
        public async Task<Result<object>> AddNewBoard(AddBoardDTO addBoardDTO)
        {
            var user = await _unitOfWork.userRepository.GetByIdAsync(addBoardDTO.UserId);
            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "User does not exist!",
                    Data = null
                };
            }
            var boardMapper = addBoardDTO.ToBoard();

            // Save board to database
            await _unitOfWork.boardRepository.AddAsync(boardMapper);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Add new board successfully" : "Add new board fail",
                Data = null
            };
        }
        public async Task<Result<object>> ArchiveBoard(Guid boardId)
        {
            // Retrieve the existing board
            var existingBoard = await _unitOfWork.boardRepository.GetByIdAsync(boardId);
            if (existingBoard == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Board not found",
                    Data = null
                };
            }

            // Change the BoardStatus to Closed
            existingBoard.Status = BoardStatus.Closed;

            // Update the board in the repository
            _unitOfWork.boardRepository.Update(existingBoard);

            // Save the changes
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Board archived successfully" : "Failed to archive board",
                Data = existingBoard
            };
        }
        public async Task<Result<object>> UnarchiveBoard(Guid boardId)
        {
            // Retrieve the existing board
            var existingBoard = await _unitOfWork.boardRepository.GetByIdAsync(boardId);
            if (existingBoard == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Board not found",
                    Data = null
                };
            }

            // Check if the board is already open
            if (existingBoard.Status != BoardStatus.Closed)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Board is not archived",
                    Data = null
                };
            }

            // Change the BoardStatus back to Open
            existingBoard.Status = BoardStatus.Open;

            // Update the board in the repository
            _unitOfWork.boardRepository.Update(existingBoard);

            // Save the changes
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Board unarchived successfully" : "Failed to unarchive board",
                Data = existingBoard
            };
        }

        public async Task<Result<object>> UpdateBoard(UpdateBoardDTO updateBoardDTO)
        {
            var board = await _unitOfWork.boardRepository.GetByIdAsync(updateBoardDTO.Id);
            if (board == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Board not found",
                    Data = null
                };
            }

            board.Title = updateBoardDTO.Title;
            board.Description = updateBoardDTO.Description;

            _unitOfWork.boardRepository.Update(board);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Update board successfully" : "Update board failed",
                Data = null
            };
        }

        public async Task<Result<object>> ChangeBoardName(ChangeBoardNameDTO changeBoardNameDTO)
        {
            var board = await _unitOfWork.boardRepository.GetByIdAsync(changeBoardNameDTO.Id);
            if (board == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Board not found",
                    Data = null
                };
            }

            board.Title = changeBoardNameDTO.Title;

            _unitOfWork.boardRepository.Update(board);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Change Board Name successfully" : "Change board name failed",
                Data = null
            };
        }

        public async Task<Result<object>> DeleteBoard(Guid boardId)
        {
            // Retrieve the existing board
            var existingBoard = await _unitOfWork.boardRepository.GetByIdAsync(boardId);
            if (existingBoard == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Board not found",
                    Data = null
                };
            }

            // Soft delete the board
            _unitOfWork.boardRepository.SoftRemove(existingBoard);

            // Save the changes
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Board deleted successfully" : "Failed to delete board",
                Data = null
            };
        }

        public async Task<Result<object>> ViewArchivedCardsFromABoard(Guid boardId)
        {
            // Check if the board exists
            var existingBoard = await _unitOfWork.boardRepository.GetByIdAsync(boardId);
            if (existingBoard == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Board not found",
                    Data = null
                };
            }
            var openCards = await _unitOfWork.cardRepository.GetArchivedCardsByBoardId(boardId);

            var mappedCards = _mapper.Map<List<ViewCardDTO>>(openCards);

            return new Result<object>
            {
                Error = 0,
                Message = "Cards retrieved successfully",
                Data = mappedCards
            };
        }


    }
}
