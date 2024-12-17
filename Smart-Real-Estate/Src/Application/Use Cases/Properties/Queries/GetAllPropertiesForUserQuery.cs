using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Property.Queries
{
    public class GetAllPropertiesForUserQuery : IRequest<Result<IEnumerable<PropertyDto>>>
    {
        public Guid UserId { get; set; }
    }
}
