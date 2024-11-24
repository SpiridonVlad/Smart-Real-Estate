using MediatR;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Repositories;
using Domain.Entities;
using Domain.Common;
using Application.Use_Cases.Users.Commands;

namespace Application.Use_Cases.Users.CommandHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<string>>
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UpdateUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            UpdateUserCommandValidator validator = new UpdateUserCommandValidator();
            var validationResult = await validator.ValidateAsync(request,cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<string>.Failure(validationResult.ToString());
            }
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
