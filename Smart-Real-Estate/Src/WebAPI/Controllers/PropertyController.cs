using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Use_Cases.Commands;
using Application.DTOs;
using Domain.Common;
using Application.Use_Cases.Queries;
using Application.Use_Cases.Property.Commands;

namespace Real_Estate_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PropertyController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateProperty([FromBody] CreatePropertyCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return CreatedAtAction(nameof(GetPropertyById), new { Id = result.Data }, result.Data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAllProperties(int page,int pageSize)
        {
            var query = new GetAllPropertiesQuery { Page = page, PageSize = pageSize};
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDto>> GetPropertyById(Guid id)
        {
            var query = new GetPropertyByIdQuery { Id = id };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] UpdatePropertyCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var command = new DeletePropertyCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}
