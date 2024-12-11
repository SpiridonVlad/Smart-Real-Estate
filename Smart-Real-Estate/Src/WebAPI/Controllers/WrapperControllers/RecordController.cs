using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Use_Cases.Queries;
using Application.DTOs;
using Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities;
using Application.Use_Cases.Wrappers;

namespace Real_Estate_Management_System.Controllers.WrapperControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RecordController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;


        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<RecordDto>>> GetPagiantedListings(int page, int pageSize, [FromQuery] ListingFilter filter)
        {
            var query = new GetPaginatedRecordsQuery { Page = page, PageSize = pageSize };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RecordDto>> GetListingById(Guid id)
        {
            var query = new GetRecordByIdQuery { Id = id };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

    }
}
