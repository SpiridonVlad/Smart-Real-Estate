﻿using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Use_Cases.Commands;
using Application.Use_Cases.Queries;
using Application.DTOs;
using Domain.Common;
using Domain.Filters;
using Application.Use_Cases.Listings.Commands;
using Microsoft.AspNetCore.Authorization;

namespace Real_Estate_Management_System.Controllers
{
    [Authorize]
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

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<ListingDto>>> GetPagiantedListings(int page, int pageSize,[FromQuery] ListingFilter filter)
        {
            var query = new GetPaginatedListingsQuery { Page = page, PageSize = pageSize, Filter = filter};
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateListing(Guid id, [FromBody] UpdateListingCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The id should be identical with the command.id");
            }
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteListing(Guid id)
        {
            var command = new DeleteListingCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}
