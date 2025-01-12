using Microsoft.AspNetCore.Http;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Application.Utils;


namespace Application.Use_Cases.CommandHandlers
{
    public class CreateListingCommandHandler(IListingRepository repository, IMapper mapper, IPropertyRepository propertyRepository, IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateListingCommand, Result<Guid>>
    {
        private readonly IListingRepository repository = repository;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<Guid>> Handle(CreateListingCommand request, CancellationToken cancellationToken)
        {

            var userIdResult = JwtHelper.GetUserIdFromJwt(httpContextAccessor.HttpContext);

            var propertyId = await ValidatePropertyId(request.PropertyId);

            if (!userIdResult.IsSuccess)
            {
                return Result<Guid>.Failure(userIdResult.ErrorMessage);
            }
            if (!propertyId.IsSuccess)
            {
                return Result<Guid>.Failure(propertyId.ErrorMessage);
            }
            Console.WriteLine("User ID: " + userIdResult.Data);
            request.UserId = userIdResult.Data;
            var listing = mapper.Map<Listing>(request);
            var result = await repository.AddAsync(listing);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }

        public async Task<Result<Guid>> ValidatePropertyId(Guid propertyId)
        {
            var result = await propertyRepository.GetByIdAsync(propertyId);
            if (result == null)
            {
                return Result<Guid>.Failure("Property not found");
            }
            return Result<Guid>.Success(propertyId);
        }

    }
}
