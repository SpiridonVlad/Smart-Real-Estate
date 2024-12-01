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
            UpdatePropertyCommandValidator validator = new();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<string>.Failure(validationResult.ToString());
            }

            var propertyResult = await propertyRepository.GetByIdAsync(request.Id);
            if (!propertyResult.IsSuccess)
            {
                return Result<string>.Failure(propertyResult.ErrorMessage);
            }

            var property = propertyResult.Data;
            mapper.Map(request, property);

            var updateResult = await propertyRepository.UpdateAsync(property);
            if (updateResult.IsSuccess)
            {
                return Result<string>.Success("");
            }
            return Result<string>.Failure(updateResult.ErrorMessage);
            
        }
    }
}
