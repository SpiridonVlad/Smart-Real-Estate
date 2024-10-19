using Domain.Entities;

namespace Domain.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Guid id);
    }
}
