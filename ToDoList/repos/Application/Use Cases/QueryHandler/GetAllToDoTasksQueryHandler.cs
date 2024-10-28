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
    public class GetAllToDoTasksQueryHandler : IRequestHandler<GetAllToDoTasksQuery, IEnumerable<ToDoTaskDto>>
    {
        private readonly IMapper mapper;
        private readonly IToDoTaskRepository repository;

        public GetAllToDoTasksQueryHandler( IMapper mapper, IToDoTaskRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public async Task<IEnumerable<ToDoTaskDto>> Handle(GetAllToDoTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await repository.GetAllAsync();
            return mapper.Map<IEnumerable<ToDoTaskDto>>(tasks);
        }
    }
}
