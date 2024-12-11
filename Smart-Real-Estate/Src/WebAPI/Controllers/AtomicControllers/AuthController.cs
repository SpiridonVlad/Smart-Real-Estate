﻿using Application.Authentication;
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
    }
}