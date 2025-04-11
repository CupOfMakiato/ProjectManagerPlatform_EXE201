using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Interfaces;
using Server.Application.Services;
using Server.Contracts.Abstractions.Shared;
using System.Threading.Tasks;
using System;
using Server.Contracts.DTO.Subcription;
using Server.Contracts.Abstractions.RequestAndResponse.Subcription;
using Server.Application.Validations.SubcriptionValidations;
using System.Linq;
using Server.Application.Mappers.SubcriptionExtension;
using Server.Application.Validations.BoardValidations;
using Server.Contracts.Abstractions.RequestAndResponse.Board;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.API.Controllers
{
    [Route("api/subcription")]
    [ApiController]
    public class SubcriptionController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISubcriptionService _subcriptionService;

        public SubcriptionController(IMapper mapper, ISubcriptionService subcriptionService)
        {
            _mapper = mapper;
            _subcriptionService = subcriptionService;
        }

        [HttpGet("ViewAllSubcriptions")]
        [ProducesResponseType(200, Type = typeof(ViewSubcriptionDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllOpensubcriptions()
        {
            var subcription = await _subcriptionService.ViewAllSubcriptions();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(subcription);
        }

        [HttpPost("AddNewSubcription")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> AddNewSubcription([FromForm] AddNewSubcriptionRequest req)
        {
            var validator = new AddNewSubcriptionRequestValidator();
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

            var subcriptionMapper = req.ToAddNewSubcriptionDTO();
            var result = await _subcriptionService.AddNewSubcription(subcriptionMapper);

            return Ok(result);
        }

        [HttpPut("UpdateSubcription")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> UpdateSubcription([FromForm] UpdateSubcriptionRequest req)
        {
            var validator = new UpdateSubcriptionRequestValidator();
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

            var subcriptionMapper = req.ToUpdateSubcriptionDTO();

            var result = await _subcriptionService.UpdateSubcription(subcriptionMapper);

            return Ok(result);
        }
        [HttpDelete("DeleteSubcription/{subcriptionId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> DeleteBoard(Guid subcriptionId)
        {
            var result = await _subcriptionService.DeleteSubcription(subcriptionId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }
    }
}
