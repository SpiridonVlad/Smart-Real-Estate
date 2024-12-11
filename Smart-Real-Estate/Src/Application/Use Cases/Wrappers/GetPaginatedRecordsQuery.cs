using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Wrappers
{
    public class GetPaginatedRecordsQuery : IRequest<Result<IEnumerable<RecordDto>>>
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
