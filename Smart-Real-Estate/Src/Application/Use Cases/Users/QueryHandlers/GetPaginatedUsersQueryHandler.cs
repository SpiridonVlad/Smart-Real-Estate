using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;


namespace Application.Use_Cases.QueryHandlers
{
    public class GetPaginatedUsersQueryHandler(IMapper mapper, IUserRepository repository) : IRequestHandler<GetPaginatedUsersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IMapper mapper = mapper;
        private readonly IUserRepository repository = repository;

        public async Task<Result<IEnumerable<UserDto>>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
        {
            var filterExpression = request.Filters.BuildFilterExpression();

            var usersResult = await repository.GetPaginatedAsync(
                request.Page,
                request.PageSize,
                filterExpression
            );

            if (usersResult.IsSuccess)
            {
                var userDtos = mapper.Map<IEnumerable<UserDto>>(usersResult.Data);
                return Result<IEnumerable<UserDto>>.Success(userDtos);
            }

            return Result<IEnumerable<UserDto>>.Failure(usersResult.ErrorMessage);
        }
    }
}
