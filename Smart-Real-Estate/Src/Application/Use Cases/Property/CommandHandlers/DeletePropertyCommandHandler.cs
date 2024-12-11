using Application.Use_Cases.Property.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Property.CommandHandlers
{
    public class DeletePropertyCommandHandler(IPropertyRepository repository, IMapper mapper) : IRequestHandler<DeletePropertyCommand, Result<string>>
    {
        private readonly IPropertyRepository repository = repository;

        public async Task<Result<string>> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await repository.DeleteAsync(request.Id);
            if (property.IsSuccess)
            {
                return Result<string>.Success("Property deleted successfully");
            }
            else
            {
                return Result<string>.Failure(property.ErrorMessage);
            }
        }
    }
}
