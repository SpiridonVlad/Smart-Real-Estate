using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;


namespace Application.Use_Cases.CommandHandlers
{
    public class UpdateToDoTaskCommandHandler : IRequestHandler<UpdateToDoTaskCommand>
    {
        private readonly IToDoTaskRepository repository;
        private readonly IMapper mapper;

        public UpdateToDoTaskCommandHandler(IToDoTaskRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task Handle(UpdateToDoTaskCommand request, CancellationToken cancellationToken)
        {
            var toDoTask = mapper.Map<ToDoTask>(request);
            await repository.UpdateAsync(toDoTask);
        }
    }
}
