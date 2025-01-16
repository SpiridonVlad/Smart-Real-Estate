using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAddressRepository
    {
        Task<Result<Address>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<Address>>> GetPaginatedAsync(int page, int pageSize);
        Task<Result<Guid>> AddAsync(Address address);
        Task<Result<object>> UpdateAsync(Address address);
        Task<Result<object>> DeleteAsync(Guid id);
    }
}
