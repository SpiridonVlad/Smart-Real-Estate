using Application.Authentication;
using Application.Use_Cases.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Real_Estate_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var userId = await mediator.Send(command);
            return Ok(userId);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var token = await mediator.Send(command);
            return Ok(token);
        }

        [HttpGet("Confirm")]
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
