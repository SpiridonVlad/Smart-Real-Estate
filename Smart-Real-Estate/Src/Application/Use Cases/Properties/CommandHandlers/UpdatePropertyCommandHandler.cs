using MediatR;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Repositories;
using Domain.Common;


namespace Application.Use_Cases.CommandHandlers
{
    public class UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper) : IRequestHandler<UpdatePropertyCommand, Result<string>>
    {
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<string>> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var propertyResult = await propertyRepository.GetByIdAsync(request.Id);
            if (!propertyResult.IsSuccess)
            {
                return Result<string>.Failure(propertyResult.ErrorMessage);
            }

            var property = propertyResult.Data;
            mapper.Map(request, property);

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
            var updateResult = await propertyRepository.UpdateAsync(property);
            if (updateResult.IsSuccess)
            {
                return Result<string>.Success("");
            }
            return Result<string>.Failure(updateResult.ErrorMessage);
            
        }
    }
}
