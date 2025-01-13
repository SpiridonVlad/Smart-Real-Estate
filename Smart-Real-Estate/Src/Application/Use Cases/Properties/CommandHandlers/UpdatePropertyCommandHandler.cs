using MediatR;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Repositories;
using Domain.Common;
using Domain.Entities;


namespace Application.Use_Cases.CommandHandlers
{
    public class UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper,IAddressRepository addressRepository) : IRequestHandler<UpdatePropertyCommand, Result<string>>
    {
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IAddressRepository addressRepository = addressRepository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<string>> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var existingAddress = await addressRepository.GetByIdAsync(request.AddressId.Value);
            if (existingAddress == null)
            {
                return Result<string>.Failure("The specified AddressId does not exist.");
            }

            var address = new Address
            {
                Id = request.AddressId.Value,
                Country = request.Address.Country,
                Street = request.Address.Street,
                City = request.Address.City,
                State = request.Address.State,
                PostalCode = request.Address.PostalCode,
            };

            var newRequest = new UpdatePropertyCommand
            {
                Id = request.Id,
                Title = request.Title,
                ImageIds = request.ImageIds,
                UserId = request.UserId,
                Type = request.Type,
                Features = request.Features
            };
            Console.WriteLine(address.Id);
            var property = mapper.Map<Domain.Entities.Property>(newRequest);
            var updateResult = await propertyRepository.UpdateAsync(property);
            var addressUpdateResult = await addressRepository.UpdateAsync(address);

            if (updateResult.IsSuccess && addressUpdateResult.IsSuccess)
            {
                return Result<string>.Success("");
            }

            return Result<string>.Failure(updateResult.ErrorMessage);
            
        }
    }
}
