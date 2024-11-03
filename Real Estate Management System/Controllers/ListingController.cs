using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Use_Cases.Commands;
using Application.DTOs;
using Domain.Common;
using Application.Use_Cases.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoList.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IMediator mediator;

        public ListingController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateListing([FromBody] CreateListingCommand command)
        {
            var result = await mediator.Send(command);
            return CreatedAtAction(nameof(GetListingById), new { Id = result.Data }, result.Data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListingDto>>> GetAllListings()
        {
            var query = new GetAllListingsQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListingDto>> GetListingById(Guid id)
        {
            var query = new GetListingByIdQuery { Id = id };
            var result = await mediator.Send(query);
            return Ok(result);
        }

    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteListing(Guid id)
    //    {
    //        var command = new DeleteListingCommand { Id = id };
    //        await mediator.Send(command);
    //        return NoContent();
    //    }

    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> UpdateListing(Guid id, [FromBody] UpdateListingCommand command)
    //    {
    //        if (id != command.Id)
    //        {
    //            return BadRequest();
    //        }
    //        await mediator.Send(command);
    //        return NoContent();
    //    }
    //}
}
