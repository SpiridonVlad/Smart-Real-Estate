using Domain.Entities;
using Domain.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<Property> Properties { get; set; }
        public required DbSet<Listing> Listings { get; set; }
        public required DbSet<User> Users { get; set; }
        public required DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("addresses");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Street).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.PostalCode).IsRequired();
                entity.Property(e => e.Country).IsRequired();
                entity.Property(e => e.AdditionalInfo);
            });

            modelBuilder.Entity<Property>(entity =>
            {
                // Define the table name and primary key
                entity.ToTable("properties");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();

                // AddressId and navigation property
                entity.Property(e => e.AddressId)
                    .HasColumnType("uuid")
                    .IsRequired();

                entity.HasOne(e => e.Address)
                    .WithMany()
                    .HasForeignKey(e => e.AddressId)
                    .OnDelete(DeleteBehavior.Cascade);

                // UserId and navigation property
                entity.Property(e => e.UserId)
                    .HasColumnType("uuid")
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany() 
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // ImageId
                entity.Property(e => e.ImageId)
                    .IsRequired();

                // PropertyType enum
                entity.Property(e => e.Type)
                    .HasConversion<string>()
                    .IsRequired();

                // PropertyFeatures configuration with a dictionary for Features
                entity.OwnsOne(e => e.Features, features =>
                {
                    features.Property(f => f.Features)
                        .HasColumnType("jsonb")
                        .HasConversion(
                            v => JsonConvert.SerializeObject(v),
                            v => JsonConvert.DeserializeObject<Dictionary<PropertyFeatureType, int>>(v)
                        )
                        .Metadata.SetValueComparer(new ValueComparer<Dictionary<PropertyFeatureType, int>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value)),
                            c => c.ToDictionary(k => k.Key, v => v.Value)
                        ));
                });
            });

            modelBuilder.Entity<Listing>(static entity =>
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
                    .HasColumnType("int");

                entity.Property(e => e.PublicationDate)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsRequired(false);

                entity.OwnsOne(e => e.Features, features =>
                {
                    features.Property(f => f.Features)
                        .HasColumnType("jsonb")
                        .HasConversion(
                            v => JsonConvert.SerializeObject(v), 
                            v => JsonConvert.DeserializeObject<Dictionary<ListingAssetss, int>>(v) 
                        )
                        .Metadata.SetValueComparer(new ValueComparer<Dictionary<ListingAssetss, int>>(
                            (c1, c2) => c1.SequenceEqual(c2), 
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value)), 
                            c => c.ToDictionary(k => k.Key, v => v.Value) 
                        ));
                });
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

                entity.Property(e => e.Verified).IsRequired();
                entity.Property(e => e.Rating).IsRequired();

                entity.Property(e => e.Type)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(e => e.PropertyHistory)
                    .HasColumnType("jsonb")
                    .IsRequired(false)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<List<Guid>>(v)
                    )
                    .Metadata
                    .SetValueComparer(new ValueComparer<List<Guid>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()
                    ));

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}
