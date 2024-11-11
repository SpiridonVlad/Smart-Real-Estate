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
            var validationResult = await validator.ValidateAsync(request);
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

            // Verificăm dacă mapper-ul a funcționat corect
            if (!property.Address.Street.Equals(request.Address.Street) ||
                !property.Address.City.Equals(request.Address.City) ||
                !property.Address.State.Equals(request.Address.State) ||
                !property.Address.PostalCode.Equals(request.Address.PostalCode) ||
                !property.Address.Country.Equals(request.Address.Country) ||
                !property.ImageId.Equals(request.ImageId) ||
                !property.UserId.Equals(request.UserId) ||
                !property.Type.Equals(request.Type))
            {
                return Result<string>.Failure("Mapping failed.");
            }

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
