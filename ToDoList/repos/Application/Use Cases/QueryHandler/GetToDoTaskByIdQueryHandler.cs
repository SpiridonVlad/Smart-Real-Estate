using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.QueryHandler
{
    public class GetToDoTaskByIdQueryHandler : IRequestHandler<GetToDoTaskByIdQuery, ToDoTaskDto>
    {
        private readonly IMapper mapper;
        private readonly IToDoTaskRepository repository;

        public GetToDoTaskByIdQueryHandler(IMapper mapper, IToDoTaskRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public async  Task<ToDoTaskDto> Handle(GetToDoTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await repository.GetByIdAsync(request.Id);
            return mapper.Map<ToDoTaskDto>(task);
        }
    }
}
