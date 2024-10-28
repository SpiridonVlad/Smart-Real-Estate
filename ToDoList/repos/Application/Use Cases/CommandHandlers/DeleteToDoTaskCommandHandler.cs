using Application.Use_Cases.Commands;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.CommandHandlers
{
    public class DeleteToDoTaskCommandHandler : IRequestHandler<DeleteToDoTaskCommand>
    {
        private readonly IToDoTaskRepository repository;

        public DeleteToDoTaskCommandHandler(IToDoTaskRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(DeleteToDoTaskCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteAsync(request.Id);
        }

        
    }
}
