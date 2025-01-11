using Application.DTOs;
using Application.Use_Cases.Property.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Property.QueryHandlers
{
    public class GetAllPropertiesForUserQueryHandler(IMapper mapper, IPropertyRepository propertyRepository) : IRequestHandler<GetAllPropertiesForUserQuery, Result<IEnumerable<PropertyDto>>>
    {
        private readonly IMapper mapper = mapper;
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        public async Task<Result<IEnumerable<PropertyDto>>> Handle(GetAllPropertiesForUserQuery request, CancellationToken cancellationToken)
        {
            var properties = await propertyRepository.GetAllForUserAsync(request.UserId);
            if (properties.IsSuccess)
            {
                return Result<IEnumerable<PropertyDto>>.Success(mapper.Map<IEnumerable<PropertyDto>>(properties.Data));
            }
            return Result<IEnumerable<PropertyDto>>.Failure("Properties not found");
        }
    }
}
