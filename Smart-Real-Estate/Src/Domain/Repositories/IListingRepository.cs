using Domain.Common;
using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IListingRepository
    {
        Task<Result<Listing>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<Listing>>> GetPaginatedAsync(int page, int pageSize, Expression<Func<Listing, bool>>? filter);
        Task<Result<IEnumerable<Listing>>> GetAllForUser(Guid userId);
        Task<Result<object>> DeleteUsersListings(Guid userId);
        Task<Result<Guid>> AddAsync(Listing listing);
        Task<Result<object>> UpdateAsync(Listing listing);
        Task<Result<object>> DeleteAsync(Guid id);
    }
}
