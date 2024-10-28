using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetToDoTaskByIdQuery : IRequest<ToDoTaskDto>
    {
        public Guid Id { get; set; }
    }
}
