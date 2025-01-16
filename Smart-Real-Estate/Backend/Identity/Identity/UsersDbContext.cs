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

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(100); 

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.Verified)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(u => u.Rating)
                    .HasColumnType("TEXT")
                    .HasDefaultValue(0m)
                    .IsRequired();

                entity.Property(u => u.PropertyHistory)
                    .HasConversion(
                        v => string.Join(",", v ?? new List<Guid>()),
                        v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());

                entity.Property(u => u.PropertyWaitingList)
                    .HasConversion(
                        v => string.Join(",", v ?? new List<Guid>()),
                        v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());

                entity.Property(u => u.ChatId)
                    .HasConversion(
                        v => string.Join(",", v ?? new List<Guid>()),
                        v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());


            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
