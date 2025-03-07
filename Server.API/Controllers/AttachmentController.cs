using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Interfaces;
using Server.Application.Mappers.AttachmentExtension;
using Server.Application.Validations.AttachmentValidations;
using Server.Application.Validations.CardValidations;
using Server.Contracts.Abstractions.RequestAndResponse.Attachment;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using Server.Contracts.Abstractions.Shared;

namespace Server.API.Controllers
{
    [Route("api/attachment")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public AttachmentController(IMapper mapper, IAttachmentService attachmentService)
        {
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        [HttpPut("ChangeAttachmentName")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ChangeAttachmentName([FromForm] ChangeAttachmentNameRequest req)
        {
            var validator = new ChangeAttachmentNameRequestValidator();
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

            var attachmentMapper = req.ToChangeAttachmentNameDTO();

            var result = await _attachmentService.ChangeAttachmentName(attachmentMapper);

            return Ok(result);
        }

        [HttpPut("MakeCover/{attachmentId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> MakeCover(Guid attachmentId)
        {
            var result = await _attachmentService.MakeCover(attachmentId);

            if (result.Error == 1)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("RemoveCover/{attachmentId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> RemoveCover(Guid attachmentId)
        {
            var result = await _attachmentService.RemoveCover(attachmentId);

            if (result.Error == 1)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
