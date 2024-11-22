using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;


namespace Application.Use_Cases.QueryHandlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository repository;

        public GetUserByIdQueryHandler(IMapper mapper, IUserRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userResult = await repository.GetByIdAsync(request.Id);
            if (userResult.IsSuccess)
            {
                return Result<UserDto>.Success(mapper.Map<UserDto>(userResult.Data));
            }
            return Result<UserDto>.Failure(userResult.ErrorMessage);

        }
    }
}
