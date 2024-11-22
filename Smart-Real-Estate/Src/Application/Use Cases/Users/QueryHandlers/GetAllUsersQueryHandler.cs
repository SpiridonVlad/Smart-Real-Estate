using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;


namespace Application.Use_Cases.QueryHandlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository repository;

        public GetAllUsersQueryHandler(IMapper mapper, IUserRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var usersResult = await repository.GetPaginatedAsync(request.Page,request.PageSize);
            if (usersResult.IsSuccess)
            {
                var userDtos = mapper.Map<IEnumerable<UserDto>>(usersResult.Data);
                return Result<IEnumerable<UserDto>>.Success(userDtos);
            }
            return Result<IEnumerable<UserDto>>.Failure(usersResult.ErrorMessage);
        }
    }
}
