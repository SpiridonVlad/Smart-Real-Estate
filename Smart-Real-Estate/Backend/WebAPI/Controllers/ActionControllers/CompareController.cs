using Application.Use_Cases.Actions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Real_Estate_Management_System.Controllers.ActionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompareController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;
        [HttpGet]
        public async Task<IActionResult> CompareProperties([FromQuery] CompareTwoPropertiesCommand command)
        {
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Data);
        }
    }
}
