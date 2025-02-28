using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Interfaces;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Card;
using Server.Contracts.DTO.Column;

//namespace Server.API.Controllers
//{
//    [Route("api/column")]
//    [ApiController]
//    public class ColumnController : ControllerBase
//    {
//        private readonly IMapper _mapper;
//        private readonly IColumnService _columnService;

//        public ColumnController(IMapper mapper, IColumnService columnService)
//        {
//            _mapper = mapper;
//            _columnService = columnService;
//        }

//        [HttpGet("ViewAllColumns")]
//        [ProducesResponseType(200, Type = typeof(ViewColumnDTO))]
//        [ProducesResponseType(400, Type = typeof(Result<object>))]
//        public async Task<IActionResult> ViewAllColumns()
//        {
//            var result = await _columnService.ViewAllColumns();

//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);
//            return Ok(result);
//        }

//        [HttpGet("ViewColumnById/{columnId}")]
//        [ProducesResponseType(200, Type = typeof(ViewColumnDTO))]
//        [ProducesResponseType(400, Type = typeof(Result<object>))]
//        public async Task<IActionResult> ViewColumnById(Guid columnId)
//        {
//            var result = await _columnService.ViewColumnById(columnId);

//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            return Ok(result);
//        }
//    }
//}
