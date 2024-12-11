using MediatR;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Repositories;
using Domain.Entities;
using Domain.Common;
using Application.Use_Cases.Users.Commands;

namespace Application.Use_Cases.Users.CommandHandlers
{
    public class UpdateUserCommandHandler(IUserRepository repository, IMapper mapper) : IRequestHandler<UpdateUserCommand, Result<string>>
    {
        private readonly IUserRepository repository = repository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(request);
            var result = await repository.UpdateAsync(user);
            if (result.IsSuccess)
            {
                return Result<string>.Success("User updated successfully");
            }
  
            return Result<string>.Failure(result.ErrorMessage);
            
        }
    }
}
