using Application.Use_Cases.Property.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.Property.CommandHandlers
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, Result<string>>
    {
        private readonly IPropertyRepository repository;
        private readonly IMapper mapper;

        public DeletePropertyCommandHandler(IPropertyRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            DeletePropertyCommandValidator validator = new DeletePropertyCommandValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result<string>.Failure(validationResult.ToString());
            }

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
