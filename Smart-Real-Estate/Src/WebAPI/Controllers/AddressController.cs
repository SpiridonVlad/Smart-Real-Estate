using Application.DTOs;
using Application.Use_Cases.Addresses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Real_Estate_Management_System.Controllers
{
    [Route("api/v1/address")]
    [ApiController]
    public class AddressController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDto>> GetAddressById(Guid id)
        {
            var query = new GetAddressByIdQuery { Id = id };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }
    }
}
