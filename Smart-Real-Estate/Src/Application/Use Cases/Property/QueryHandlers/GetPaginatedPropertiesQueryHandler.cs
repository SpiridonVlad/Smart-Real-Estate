using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Types;
using MediatR;
using Newtonsoft.Json;

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
            var filterExpression = request.Filter.BuildFilterExpression();

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
                    var minValues = request.Filter.PropertyFeaturesMinValues;
                    var maxValues = request.Filter.PropertyFeaturesMaxValues;

                    Dictionary<PropertyFeatureType, int> features = property.Features;

                    bool isValidProperty = true;
                    //foreach (var feature in features)
                    //{

                    //    if (minValues.TryGetValue(feature.Key, out int minValue) )
                    //    {

                    //        Console.WriteLine("KEy:", feature.Key);
                    //        Console.WriteLine(feature.Value);
                    //        Console.WriteLine(minValue);
                    //        int featureValue = feature.Value;

                    //        if (featureValue < minValue)
                    //        {
                    //            isValidProperty = false;
                    //            break;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        isValidProperty = false;
                    //        break;
                    //    }
                    //}

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

                    if (isValidProperty)
                    {
                        Properties.Add(mapper.Map<PropertyDto>(property));
                    }
                }

                return Result<IEnumerable<PropertyDto>>.Success(mapper.Map<IEnumerable<PropertyDto>>(Properties));
            }

            return Result<IEnumerable<PropertyDto>>.Failure(propertiesResult.ErrorMessage);
        }

    }
}
