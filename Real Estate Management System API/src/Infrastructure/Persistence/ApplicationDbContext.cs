using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("properties");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Address)
                    .IsRequired();

                entity.Property(e => e.Surface)
                    .IsRequired();

                entity.Property(e => e.Rooms)
                    .IsRequired();

                entity.Property(e => e.Type)
                    .HasConversion<string>() 
                    .IsRequired();

                entity.Property(e => e.HasGarden)
                    .IsRequired();

                entity.Property(e => e.HasGarage)
                    .IsRequired();

                entity.Property(e => e.HasPool)
                    .IsRequired();

                entity.Property(e => e.HasBalcony)
                    .IsRequired();

                // Configure UserId as a foreign key
                entity.Property(e => e.UserId)
                    .HasColumnType("uuid")
                    .IsRequired();
                // Configure Images as a one-to-many relationship
                entity.Property(e => e.ImageId)
                       .IsRequired();
            });

            modelBuilder.Entity<Listing>(entity =>
            {
                entity.ToTable("listings");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.PropertyId)
                    .HasColumnType("uuid")
                    .IsRequired();
                entity.Property(e => e.UserId)
                    .HasColumnType("uuid")
                    .IsRequired();
                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(e => e.PublicationDate).IsRequired();
                entity.Property(e => e.IsSold).IsRequired();
                entity.Property(e => e.IsHighlighted).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Verified)
                    .IsRequired();

                entity.Property(e => e.Rating)
                .IsRequired();


                entity.Property(e => e.IsAdmin)
                    .IsRequired();
            });
        }
    }
}
