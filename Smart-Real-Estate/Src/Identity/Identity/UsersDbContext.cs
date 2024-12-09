using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
