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

                entity.Property(e => e.Surface)
                    .IsRequired();

                entity.Property(e => e.Rooms)
                    .IsRequired();

                entity.Property(e => e.Type)
                    .HasConversion<string>() // Store PropertyType enum as string in the database
                    .IsRequired();

                // Configure Address as a one-to-one relationship
                entity.HasOne(e => e.Address)
                    .WithOne()
                    .HasForeignKey<Property>(p => p.AddressId) // Ensure Property entity has AddressId as a foreign key
                    .IsRequired();

                // Configure PropertyFeatures as an owned type
                entity.OwnsOne(e => e.Features, features =>
                {
                    features.Property(f => f.Features)
                        .HasColumnType("jsonb") // Assuming storage as JSON in PostgreSQL
                        .IsRequired();
                });

                // Configure Images as a one-to-many relationship
                entity.HasMany(e => e.Images)
                    .WithOne(i => i.Property)
                    .HasForeignKey(i => i.PropertyId)
                    .IsRequired();
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("addresses");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AdditionalInfo)
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("images");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Data)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(200);

                entity.HasOne(i => i.Property)
                    .WithMany(p => p.Images)
                    .HasForeignKey(i => i.PropertyId)
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

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasConversion<string>(); 

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<string>(); 

                entity.Property(e => e.verified)
                    .IsRequired();

                entity.Property(e => e.rating)
                    .HasColumnType("decimal(3,2)")
                    .HasDefaultValue(0);
            });
        }
    }
}
