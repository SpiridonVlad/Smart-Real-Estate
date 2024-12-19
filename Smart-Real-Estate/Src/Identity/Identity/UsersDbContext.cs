using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Verified).HasDefaultValue(false);
                entity.Property(u => u.Rating).HasColumnType("decimal(5, 2)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
