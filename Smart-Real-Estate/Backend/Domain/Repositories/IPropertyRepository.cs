using Domain.Common;
using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IPropertyRepository
    {
        Task<Result<Property>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<Property>>> GetPaginatedAsync(int page,int pageSize, Expression<Func<Property, bool>>? filter = null);
        Task<Result<IEnumerable<Property>>> GetAllForUserAsync(Guid userId);
        Task<Result<object>> DeleteUsersPropertys(Guid userId);
        Task<Result<Guid>> AddAsync(Property property);
        Task<Result<object>> UpdateAsync(Property property);
        Task<Result<object>> DeleteAsync(Guid id);
    }
}
