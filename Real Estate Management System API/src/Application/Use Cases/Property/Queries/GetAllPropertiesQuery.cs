using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetAllPropertiesQuery : IRequest<Result<IEnumerable<PropertyDto>>>
    {
    }
}
