using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Wrappers
{
    public class GetRealtyQuery : IRequest<Result<IEnumerable<RealtyDto>>>
    {
        public Guid Id { get; set; }
    }
}
