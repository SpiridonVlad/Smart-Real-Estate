using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<Book>(

                    entity =>
                    {
                        entity.ToTable("books");
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.Id)
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .ValueGeneratedOnAdd();
                        entity.Property(entity => entity.Title).IsRequired().HasMaxLength(200);
                        entity.Property(entity => entity.Author).IsRequired().HasMaxLength(100);
                        entity.Property(entity => entity.ISBN).IsRequired().HasMaxLength(13);
                        entity.Property(entity => entity.PublicationDate).IsRequired();
                    }
                );
        }

    }
}
