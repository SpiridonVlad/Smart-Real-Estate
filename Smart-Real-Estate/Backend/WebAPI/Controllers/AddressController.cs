using Application.DTOs;
using Application.Use_Cases.Addresses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Real_Estate_Management_System.Controllers
{
    [Authorize]
    [Route("api/v1/Address")]
    [ApiController]
    public class AddressController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpGet("{id:guid}")]
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
