using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.CommandHandlers
{
    public class CreateToDoTaskCommandHandler : IRequestHandler<CreateToDoTaskCommand, Guid>
    {
        private readonly IToDoTaskRepository repository;
        private readonly IMapper mapper;

        public CreateToDoTaskCommandHandler(IToDoTaskRepository repository, IMapper mapper) 
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(CreateToDoTaskCommand request, CancellationToken cancellationToken)
        {
            var task = mapper.Map<ToDoTask>(request);
            return await repository.AddAsync(task);

        }

    }
}
