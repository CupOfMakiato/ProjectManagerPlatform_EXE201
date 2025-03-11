using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common;
using Server.Application.Interfaces;
using Server.Application.Mappers.BoardExtension;
using Server.Application.Validations.BoardValidations;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Card;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.API.Controllers
{
    [Route("api/board")]
    [ApiController]
    public class BoardController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBoardService _boardService;

        public BoardController(IMapper mapper, IBoardService boardService)
        {
            _mapper = mapper;
            _boardService = boardService;
        }
        [HttpGet("ViewAllBoardsPagin")]
        [ProducesResponseType(200, Type = typeof(ViewBoardDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllBoardsPagin(int pageIndex = 0, int pageSize = 10)
        {
            var board = await _boardService.ViewAllBoardsPagin(pageIndex, pageSize);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(board);
        }

        [HttpGet("ViewBoardById/{boardId}")]
        [ProducesResponseType(200, Type = typeof(ViewBoardDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewBoardById(Guid boardId)
        {
            var board = await _boardService.ViewBoardById(boardId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(board);
        }

        [HttpPost("AddNewBoard")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> AddNewBoard([FromForm] AddNewBoardRequest req)
        {
            var validator = new AddNewBoardRequestValidator();
            var validatorResult = validator.Validate(req);
            if (validatorResult.IsValid == false)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Missing value!",
                    Data = validatorResult.Errors.Select(x => x.ErrorMessage),
                });
            }


            req.Id = Guid.NewGuid();

            var boardMapper = req.ToAddBoardDTO();
            var result = await _boardService.AddNewBoard(boardMapper);

            return Ok(result);
        }
        [HttpGet("ViewAllClosedBoardsPagin")]
        [ProducesResponseType(200, Type = typeof(Pagination<ViewBoardDTO>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllClosedBoardsPagin(int pageIndex = 0, int pageSize = 10)
        {
            var closedBoards = await _boardService.ViewAllClosedBoardsPagin(pageIndex, pageSize);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(closedBoards);
        }

        [HttpGet("ViewAllOpenBoards")]
        [ProducesResponseType(200, Type = typeof(ViewBoardDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllOpenBoards()
        {
            var board = await _boardService.ViewAllOpenBoards();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(board);
        }

        [HttpGet("ViewAllClosedBoards")]
        [ProducesResponseType(200, Type = typeof(ViewBoardDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllClosedBoards()
        {
            var board = await _boardService.ViewAllClosedBoards();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(board);
        }

        [HttpPost("ArchiveBoard/{boardId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ArchiveBoard(Guid boardId)
        {
            var result = await _boardService.ArchiveBoard(boardId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpPost("UnarchiveBoard/{boardId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> UnarchiveBoard(Guid boardId)
        {
            var result = await _boardService.UnarchiveBoard(boardId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpPost("UpdateBoard")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> UpdateBoard([FromForm] UpdateBoardRequest req)
        {
            var validator = new UpdateBoardRequestValidator();
            var validatorResult = validator.Validate(req);
            if (!validatorResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Invalid input!",
                    Data = validatorResult.Errors.Select(x => x.ErrorMessage),
                });
            }

            var boardMapper = req.ToUpdateBoardDTO();

            var result = await _boardService.UpdateBoard(boardMapper);

            return Ok(result);
        }

        [HttpPost("ChangeBoardName")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ChangeBoardName([FromForm] ChangeBoardNameRequest req)
        {
            var validator = new ChangeBoardNameRequestValidator();
            var validatorResult = validator.Validate(req);
            if (!validatorResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Invalid input!",
                    Data = validatorResult.Errors.Select(x => x.ErrorMessage),
                });
            }

            var boardMapper = req.ToChangeBoardNameDTO();

            var result = await _boardService.ChangeBoardName(boardMapper);

            return Ok(result);
        }

        [HttpDelete("DeleteBoard/{boardId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> DeleteBoard(Guid boardId)
        {
            var result = await _boardService.DeleteBoard(boardId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpGet("ViewAllCardsFromABoard/{boardId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllCardsFromABoard(Guid boardId)
        {
            var result = await _boardService.ViewAllCardsFromABoard(boardId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        // Filter

    }
}
