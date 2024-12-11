using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Wrappers
{
    public class GetRecordByIdQuery : IRequest<Result<RecordDto>>
    {
        public required Guid Id { get; set; }
    }
}
