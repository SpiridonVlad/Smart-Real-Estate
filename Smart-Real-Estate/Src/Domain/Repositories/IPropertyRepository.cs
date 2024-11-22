using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPropertyRepository
    {
        Task<Result<Property>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<Property>>> GetAllAsync();
        Task<Result<Guid>> AddAsync(Property property);
        Task<Result<object>> UpdateAsync(Property property);
        Task<Result<object>> DeleteAsync(Guid id);
    }
}
