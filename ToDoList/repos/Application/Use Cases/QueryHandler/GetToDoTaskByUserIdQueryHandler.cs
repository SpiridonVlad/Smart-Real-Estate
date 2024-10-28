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
    public class GetToDoTaskByUserIdQueryHandle : IRequestHandler<GetToDoTaskByUserIdQuery, IEnumerable<ToDoTaskDto>>
    {
        private readonly IMapper mapper;
        private readonly IToDoTaskRepository repository;

        public GetToDoTaskByUserIdQueryHandle(IMapper mapper, IToDoTaskRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public async Task<IEnumerable<ToDoTaskDto>> Handle(GetToDoTaskByUserIdQuery request, CancellationToken cancellationToken)
        {
            var tasks = await repository.GetAllAsync();
            var filteredTasks = tasks.Where(task => task.UserId == request.UserId);
            return mapper.Map<IEnumerable<ToDoTaskDto>>(filteredTasks);
        }
    }
}
