using Application.DTOs;
using Application.Filters;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Wrappers
{
    public class GetRecordQuery : IRequest<Result<IEnumerable<RecordDto>>>
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public RecordFilter? Filter { get; set; }
    }
}
