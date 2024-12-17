using Application.DTOs;
using Domain.Common;
using MediatR;


namespace Application.Use_Cases.Queries
{
    public class GetPropertyByIdQuery : IRequest<Result<PropertyDto>>
    {
        public Guid Id { get; set; }
    }
}
