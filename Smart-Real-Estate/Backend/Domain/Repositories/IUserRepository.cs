using Domain.Common;
using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Result<User>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<User>>> GetAllAsync();
        Task<Result<IEnumerable<User>>> GetPaginatedAsync(int page, int pageSize, Expression<Func<User, bool>>? filter = null);
        Task<Result<Guid>> AddAsync(User user);
        Task<Result<object>> UpdateAsync(User user);
        Task<Result<object>> DeleteAsync(Guid id);
        Task<Result<string>> Login(User user);
        Task<Result<Guid>> Register(User user, CancellationToken cancellationToken);
        string GenerateEmailConfirmationToken(string email, string username, string password);
    }
}
