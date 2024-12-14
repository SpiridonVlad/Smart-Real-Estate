using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Property.QueryHandlers
{
    public class GetPaginatedPropertiesQueryHandler(
        IMapper mapper,
        IPropertyRepository repository,
        IAddressRepository addressRepository) : IRequestHandler<GetPaginatedPropertiesQuery, Result<IEnumerable<PropertyDto>>>
    {
        private readonly IMapper mapper = mapper;
        private readonly IPropertyRepository repository = repository;
        private readonly IAddressRepository addressRepository = addressRepository;

        public async Task<Result<IEnumerable<PropertyDto>>> Handle(GetPaginatedPropertiesQuery request, CancellationToken cancellationToken)
        {
            var filterExpression = request.Filters.BuildFilterExpression();

            var propertiesResult = await repository.GetPaginatedAsync(
                request.Page,
                request.PageSize,
                filterExpression
                );

            if (propertiesResult.IsSuccess)
            {
                var Properties = new List<PropertyDto>();
                foreach (var property in propertiesResult.Data)
                {
                    var address = await addressRepository.GetByIdAsync(property.AddressId);
                    if (!address.IsSuccess)
                    {
                        return Result<IEnumerable<PropertyDto>>.Failure("Address not found.");
                    }

                    property.Address = new Address
                    {
                        City = address.Data.City,
                        Country = address.Data.Country,
                        PostalCode = address.Data.PostalCode,
                        Street = address.Data.Street,
                        State = address.Data.State,
                    };

                    Properties.Add(mapper.Map<PropertyDto>(property));
                }

                return Result<IEnumerable<PropertyDto>>.Success(mapper.Map<IEnumerable<PropertyDto>>(Properties));

            }
            return Result<IEnumerable<PropertyDto>>.Failure(propertiesResult.ErrorMessage);
        }
    }
}
