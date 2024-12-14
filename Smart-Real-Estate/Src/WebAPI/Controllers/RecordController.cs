using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Application.Use_Cases.Wrappers;

namespace Real_Estate_Management_System.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RecordController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;


        [HttpGet("Paginated")]
        public async Task<ActionResult<IEnumerable<RecordDto>>> GetPagiantedRecords(int page, int pageSize)
        {
            var query = new GetRecordQuery { Page = page, PageSize = pageSize };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RecordDto>> GetRecordsById(Guid id)
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
