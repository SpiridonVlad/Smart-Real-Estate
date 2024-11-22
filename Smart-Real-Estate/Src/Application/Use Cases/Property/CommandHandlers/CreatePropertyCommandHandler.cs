using Application.Use_Cases.Commands;
using Application.Use_Cases.Property.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Result<Guid>>
    {
        private readonly IPropertyRepository repository;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public CreatePropertyCommandHandler(IPropertyRepository repository, IMapper mapper, IUserRepository userRepository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Result<Guid>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            CreatePropertyCommandValidator validator = new CreatePropertyCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<Guid>.Failure(validationResult.ToString());
            }
            var userExists = await userRepository.GetByIdAsync(request.UserId);
            if (!userExists.IsSuccess)
            {
                return Result<Guid>.Failure("UserId does not exist.");
            }

            var property = mapper.Map<Domain.Entities.Property>(request);
            var result = await repository.AddAsync(property);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}
