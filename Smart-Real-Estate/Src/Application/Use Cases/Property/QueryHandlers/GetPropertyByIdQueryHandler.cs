using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Property.QueryHandlers
{
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, Result<PropertyDto>>
    {
        private readonly IMapper mapper;
        private readonly IPropertyRepository repository;

        public GetPropertyByIdQueryHandler(IMapper mapper, IPropertyRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<Result<PropertyDto>> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var propertyResult = await repository.GetByIdAsync(request.Id);
            if (propertyResult.IsSuccess)
            {
                return Result<PropertyDto>.Success(mapper.Map<PropertyDto>(propertyResult.Data));
            }
            return Result<PropertyDto>.Failure(propertyResult.ErrorMessage);
        }
    }
}
