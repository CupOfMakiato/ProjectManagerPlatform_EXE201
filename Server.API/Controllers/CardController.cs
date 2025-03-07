using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Server.Application.Interfaces;
using Server.Application.Mappers.CardExtension;
using Server.Application.Services;
using Server.Application.Validations.BoardValidations;
using Server.Application.Validations.CardValidate;
using Server.Application.Validations.CardValidations;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Card;

namespace Server.API.Controllers
{
    [Route("api/card")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICardService _cardService;

        public CardController(IMapper mapper, ICardService cardService)
        {
            _mapper = mapper;
            _cardService = cardService;
        }

        [HttpGet("ViewAllCards")]
        [ProducesResponseType(200, Type = typeof(ViewCardDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllCards()
        {
            var card = await _cardService.ViewAllCards();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(card);
        }
        [HttpGet("ViewAllOpenCards")]
        [ProducesResponseType(200, Type = typeof(ViewCardDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllOpenCards()
        {
            var card = await _cardService.ViewAllOpenCards();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(card);
        }
        [HttpGet("ViewAllArchivedCards")]
        [ProducesResponseType(200, Type = typeof(ViewCardDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAllArchivedCards()
        {
            var card = await _cardService.ViewAllArchivedCards();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(card);
        }

        [HttpPut("OpenCard/{cardId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> UnarchiveCard(Guid cardId)
        {
            var result = await _cardService.UnarchiveCard(cardId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpPut("ArchiveCard/{cardId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ArchiveCard(Guid cardId)
        {
            var result = await _cardService.ArchiveCard(cardId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpGet("ViewCardById/{cardId}")]
        [ProducesResponseType(200, Type = typeof(ViewCardDTO))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewCardById(Guid cardId)
        {
            var card = await _cardService.ViewCardById(cardId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(card);
        }

        [HttpPost("AddANewCard")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> AddANewCard([FromForm] AddNewCardRequest req)
        {
            var validator = new AddNewCardRequestValidator();
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

            var cardMapper = req.ToAddCardDTO();
            var result = await _cardService.AddANewCard(cardMapper);

            return Ok(result);
        }

        [HttpPut("UpdateCard")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> UpdateCard([FromForm] UpdateCardRequest req)
        {
            var validator = new UpdateCardRequestValidator();
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

            var cardMapper = req.ToUpdateCardDTO();

            var result = await _cardService.UpdateCard(cardMapper);

            return Ok(result);
        }

        [HttpPut("ChangeCardName")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ChangeCardName([FromForm] ChangeCardNameRequest req)
        {
            var validator = new ChangeCardNameRequestValidator();
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

            var cardMapper = req.ToChangeCardNameDTO();

            var result = await _cardService.ChangeCardName(cardMapper);

            return Ok(result);
        }

        [HttpPut("EditCardDescription")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> EditCardDescription([FromForm] EditCardDescriptionRequest req)
        {
            var validator = new EditCardDescriptionRequestValidator();
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

            var cardMapper = req.ToEditCardDescriptionDTO();

            var result = await _cardService.EditCardDescription(cardMapper);

            return Ok(result);
        }

        [HttpDelete("DeleteCard/{cardId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> DeleteCard(Guid cardId)
        {
            var result = await _cardService.DeleteCard(cardId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpPost("UploadFileAttachment/{cardId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> UploadFileAttachment(Guid cardId, [FromForm] UploadFileAttachmentRequest request)
        {
            var validator = new UploadFileAttachmentRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "File validation failed!",
                    Data = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var result = await _cardService.UploadFileAttachment(cardId, request.File);

            if (result == null)
            {
                return StatusCode(500, new Result<object>
                {
                    Error = 1,
                    Message = "Something went wrong, file upload failed!"
                });
            }

            if (result.Error == 1)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("MoveCardInColumn")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> MoveCardInColumn([FromQuery] MoveCardInColumnRequest request)
        {
            var validator = new MoveCardInColumnRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Invalid input!",
                    Data = validationResult.Errors.Select(x => x.ErrorMessage),
                });
            }

            var cardMapper = request.ToMoveCardInColumnDTO();
            var result = await _cardService.MoveCardInColumn(cardMapper);

            return Ok(result);
        }

        [HttpDelete("DeleteAttachment/{cardId}/{attachmentId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> DeleteAttachment(Guid cardId, Guid attachmentId)
        {
            var result = await _cardService.DeleteAttachment(cardId, attachmentId);

            if (result.Error == 1)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("MoveCardToColumn")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> MoveCardToColumn([FromQuery] MoveCardToColumnRequest request)
        {
            var validator = new MoveCardToColumnRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Invalid input!",
                    Data = validationResult.Errors.Select(x => x.ErrorMessage),
                });
            }

            var cardMapper = request.ToMoveCardToColumnDTO();
            var result = await _cardService.MoveCardToList(cardMapper);

            return Ok(result);
        }




    }
}
