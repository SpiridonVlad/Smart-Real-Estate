using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Use_Cases.Commands;
using Application.DTOs;

using Domain.Common;
using Application.Use_Cases.Queries;
using Application.Use_Cases.Users.Commands;
using Microsoft.AspNetCore.Authorization;
using Domain.Types;
using Application.Filters;

namespace Real_Estate_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateUser([FromBody] CreateUserCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return CreatedAtAction(nameof(GetUserById), new { userId = result.Data }, result.Data);
        }

        [HttpGet("Paginated")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(
            int page,
            int pageSize,
            bool? verified = null,
            UserType? type = null,
            decimal? minRating = null,
            decimal? maxRating = null,
            string? username = null,
            string? email = null)
        {
            var query = new GetPaginatedUsersQuery
            {
                Page = page,
                PageSize = pageSize,
                Filters = new UserFilter
                {
                    Verified = verified,
                    Type = type,
                    MinRating = minRating,
                    MaxRating = maxRating,
                    Username = username,
                    Email = email
                }
            };

            var result = await mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }


        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid userId)
        {
            var query = new GetUserByIdQuery { Id = userId };
            Console.WriteLine("Query: " + query.Id);
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [AuthorizeUser]
        [HttpDelete("{id:guid}")]
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

        [AuthorizeUser]
        [HttpPut("{id:guid}")]
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

        [AuthorizeUser]
        [HttpPut("{userId:guid}/add_property/{propertyId:guid}")]
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

        [AuthorizeUser]
        [HttpPut("{userId:guid}/verify")]
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