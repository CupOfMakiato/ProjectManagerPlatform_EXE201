using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Interfaces;
using Server.Application.Mappers.ColumsExtensions;
using Server.Application.Services;
using Server.Application.Validations.BoardValidations;
using Server.Application.Validations.ColumnValidations;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using Server.Contracts.Abstractions.RequestAndResponse.Column;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Column;

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IColumnsService _columnsService;

        public ColumsController(IMapper mapper, IColumnsService columnsService)
        {
            _mapper = mapper;
            _columnsService = columnsService;
        }

        [HttpGet("ViewAllColumns")]
        [ProducesResponseType(200, Type = typeof(ViewColumnDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllBoards()
        {
            var columns = await _columnsService.ViewAllColumns();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(columns);
        }

        [HttpGet("ViewColumsById/{columnId}")]
        [ProducesResponseType(200, Type = typeof(ViewColumnDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewColumnById(Guid columnId)
        {
            var column = await _columnsService.ViewColumnsById(columnId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(column);
        }

        [HttpPost("AddNewColumn")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> AddNewBoard([FromForm] AddNewColumnRequest req)
        {
            var validator = new AddNewColumsRequestValidator();
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
            var columnMapper = req.ToAddColumsDTO();
            var result = await _columnsService.AddNewColumn(columnMapper);
            return Ok(result);
        }
    }
}
