using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ToDoTaskRepository : IToDoTaskRepository
    {
        private readonly ApplicationDbContext context;

        public ToDoTaskRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Guid> AddAsync(ToDoTask task)
        {
           await context.ToDoTasks.AddAsync(task);
           await context.SaveChangesAsync();
            return task.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var ToDoTask = await context.ToDoTasks.FindAsync(id);
            context.ToDoTasks.Remove(ToDoTask);
            await context.SaveChangesAsync();

        }

        public async Task<IEnumerable<ToDoTask>> GetAllAsync()
        {
            return await context.ToDoTasks.ToListAsync();
        }

        public async Task<ToDoTask> GetByIdAsync(Guid id)
        {
            return await context.ToDoTasks.FindAsync(id);
        }

        public async Task UpdateAsync(ToDoTask task)
        {
            context.Entry(task).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
