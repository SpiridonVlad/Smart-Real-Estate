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
        public RecordFilter? Filters { get; set; } = new RecordFilter
        {
            ListingFilter = new ListingFilter(),
            PropertyFilter = new PropertyFilter(),
            AddressFilter = new AddressFilter(),
            UserFilters = new UserFilter()
        };
    }
}
