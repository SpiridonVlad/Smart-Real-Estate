
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Use_Cases.Commands;
using Application.DTOs;
using Application.Use_Cases.Queries;
namespace ToDoList.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        private readonly IMediator mediator;

        public ToDoTaskController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateToDoTask([FromBody] CreateToDoTaskCommand command)
        {
            var id = await mediator.Send(command);
            return Ok(id);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTaskDto>>> GetAllToDoTasks()
        {
            var query = new GetAllToDoTasksQuery();
            var result = await mediator.Send(query);
            return Ok(result);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTaskDto>> GetToDoTaskById(Guid id)
        {
            var query = new GetToDoTaskByIdQuery { Id=id};
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ToDoTaskDto>>> GetToDoTaskByUserId(int userId)
        {
            var query = new GetToDoTaskByUserIdQuery { UserId = userId };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteToDoTask(Guid id)
        {
            var command = new DeleteToDoTaskCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoTask(Guid id, [FromBody] UpdateToDoTaskCommand command)
        {
            if(id!= command.Id)
            {
                return BadRequest();
            }
            await mediator.Send(command);
            return NoContent();
        }

    }
}
