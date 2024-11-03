using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Result<User>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<User>>> GetAllAsync();
        Task<Result<Guid>> AddAsync(User user);
        Task<Result<object>> UpdateAsync(User user);
        Task<Result<object>> DeleteAsync(Guid id);
    }
}
