using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Property.QueryHandlers
{
    public class GetAllPropertiesQueryHandler(IMapper mapper, IPropertyRepository repository) : IRequestHandler<GetAllPropertiesQuery, Result<IEnumerable<PropertyDto>>>
    {
        private readonly IMapper mapper = mapper;
        private readonly IPropertyRepository repository = repository;

        public async Task<Result<IEnumerable<PropertyDto>>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var propertiesResult = await repository.GetAllAsync(request.Page, request.PageSize);
            if (propertiesResult.IsSuccess)
            {
                var propertyDtos = mapper.Map<IEnumerable<PropertyDto>>(propertiesResult.Data);
                return Result<IEnumerable<PropertyDto>>.Success(propertyDtos);
            }
            return Result<IEnumerable<PropertyDto>>.Failure(propertiesResult.ErrorMessage);
        }
    }
}
