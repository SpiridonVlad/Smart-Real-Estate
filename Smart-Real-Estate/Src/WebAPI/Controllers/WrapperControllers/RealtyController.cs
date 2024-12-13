using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.DTOs;
using Application.Use_Cases.Wrappers;


namespace Real_Estate_Management_System.Controllers.WrapperControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RealtyController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [AuthorizeUser]
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<IEnumerable<RealtyDto>>> GetUsersRealtys(Guid userId)
        {
            var query = new GetRealtyQuery {Id = userId };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }
    }
}
