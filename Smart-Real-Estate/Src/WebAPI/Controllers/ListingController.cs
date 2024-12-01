using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Use_Cases.Commands;
using Application.Use_Cases.Queries;
using Application.DTOs;
using Domain.Common;
using Application.Use_Cases.Listings.Commands;

namespace Real_Estate_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ListingController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateListing([FromBody] CreateListingCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return CreatedAtAction(nameof(GetListingById), new { Id = result.Data }, result.Data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListingDto>>> GetPagiantedListings(int page, int pageSize)
        {
            var query = new GetPaginatedListingsQuery { Page = page, PageSize = pageSize };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListingDto>> GetListingById(Guid id)
        {
            var query = new GetListingByIdQuery { Id = id };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateListing(Guid id, [FromBody] UpdateListingCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The id should be identical with the command.id");
            }
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListing(Guid id)
        {
            var command = new DeleteListingCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}
