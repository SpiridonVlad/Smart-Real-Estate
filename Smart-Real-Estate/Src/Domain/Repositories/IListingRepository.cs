using Domain.Common;
using Domain.Entities;
using Domain.Filters;

namespace Domain.Repositories
{
    public interface IListingRepository
    {
        Task<Result<Listing>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<Listing>>> GetPaginatedAsync(int page, int pageSize);
        Task<Result<Guid>> AddAsync(Listing listing);
        Task<Result<object>> UpdateAsync(Listing listing);
        Task<Result<object>> DeleteAsync(Guid id);

    }
}
