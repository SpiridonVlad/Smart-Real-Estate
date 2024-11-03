using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;


namespace Application.Use_Cases.CommandHandlers
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Result<Guid>>
    {
        private readonly IPropertyRepository repository;
        private readonly IMapper mapper;

        public CreatePropertyCommandHandler(IPropertyRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = mapper.Map<Property>(request);
            var result = await repository.AddAsync(property);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}
