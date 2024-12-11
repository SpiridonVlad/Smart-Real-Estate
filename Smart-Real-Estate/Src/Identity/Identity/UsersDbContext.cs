using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }
    }
}
