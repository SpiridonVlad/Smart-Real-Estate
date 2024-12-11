using Application.Authentication;
using Application.Use_Cases.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Real_Estate_Management_System.Controllers.AtomicControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var userId = await mediator.Send(command);
            return Ok(userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var token = await mediator.Send(command);
            return Ok(token);
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            var command = new ConfirmEmailCommand() { Token = token };
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Data); 
            }

            return BadRequest(result.Data); 
        }

    }
}
