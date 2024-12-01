using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Use_Cases.Commands;
using Application.DTOs;

using Domain.Common;
using Application.Use_Cases.Queries;
using Application.Use_Cases.Users.Commands;

namespace Real_Estate_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateUser([FromBody] CreateUserCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return CreatedAtAction(nameof(GetUserById), new { Id = result.Data }, result.Data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(int page, int pageSize)
        {
            var query = new GetAllUsersQuery { Page = page, PageSize = pageSize };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand { Id = id };
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The id should be identical with the command.id");
            }
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return NoContent();
        }
        [HttpPut("{userId}/add_property/{propertyId}")]
        public async Task<IActionResult> AddPropertyToHistory(Guid userId, Guid propertyId)
        {
            var command = new AddPropertyToHistoryCommand
            {
                UserId = userId,
                PropertyId = propertyId
            };
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return NoContent();
        }
        [HttpPut("{userId}/verify")]
        public async Task<IActionResult> VerifyUser(Guid userId)
        {
            var command = new VerifyUserCommand
            {
                UserId = userId,
            };
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return NoContent();
        }
    }
}