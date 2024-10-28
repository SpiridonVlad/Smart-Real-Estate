using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IToDoTaskRepository 
    {
        Task<ToDoTask> GetByIdAsync(Guid id);
        Task<IEnumerable<ToDoTask>> GetAllAsync();
        Task<Guid> AddAsync(ToDoTask task);
        Task UpdateAsync(ToDoTask task);
        Task DeleteAsync(Guid id);

    }
}
