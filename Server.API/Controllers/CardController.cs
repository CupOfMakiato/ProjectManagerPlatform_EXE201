using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Interfaces;
using Server.Application.Mappers.CardExtension;
using Server.Application.Services;
using Server.Application.Validations.BoardValidations;
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
    }
}
