using MediatR;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Repositories;
using Domain.Common;


namespace Application.Use_Cases.CommandHandlers
{
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, Result<string>>
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IMapper mapper;

        public UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            this.propertyRepository = propertyRepository;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            UpdatePropertyCommandValidator validator = new UpdatePropertyCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<string>.Failure(validationResult.ToString());
            }

            var propertyResult = await propertyRepository.GetByIdAsync(request.Id);
            if (!propertyResult.IsSuccess)
            {
                return Result<string>.Failure("Property not found.");
            }

            var property = propertyResult.Data;
            mapper.Map(request, property);

            var updateResult = await propertyRepository.UpdateAsync(property);
            if (updateResult.IsSuccess)
            {
                return Result<string>.Success("Property updated successfully");
            }
            else
            {
                return Result<string>.Failure(updateResult.ErrorMessage);
            }
        }
    }
}
