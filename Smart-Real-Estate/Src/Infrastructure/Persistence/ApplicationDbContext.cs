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
                entity.ToTable("properties");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AddressId)
                    .HasColumnType("uuid")
                    .IsRequired();

                entity.HasOne(e => e.Address)
                    .WithMany()
                    .HasForeignKey(e => e.AddressId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.UserId)
                    .HasColumnType("uuid")
                    .IsRequired();

                entity.Property(e => e.ImageId)
                    .IsRequired();

                entity.Property(e => e.Type)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(e => e.Features)
                    .HasColumnName("features")
                    .HasColumnType("jsonb") 
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v), // Convert to JSON for storage
                        v => JsonConvert.DeserializeObject<Dictionary<PropertyFeatureType, int>>(v) // Deserialize back
                    )
                    .Metadata.SetValueComparer(new ValueComparer<Dictionary<PropertyFeatureType, int>>(
                        (c1, c2) => c1.SequenceEqual(c2), // Compare dictionaries for equality
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value)), // Compute hash code
                        c => c.ToDictionary(k => k.Key, v => v.Value) // Create a deep copy
                    ));

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

                entity.Property(e => e.Features)
                        .HasColumnName("features")
                        .HasColumnType("jsonb") 
                        .HasConversion(
                            v => JsonConvert.SerializeObject(v), 
                            v => JsonConvert.DeserializeObject<Dictionary<ListingType, int>>(v) 
                        )
                        .Metadata.SetValueComparer(new ValueComparer<Dictionary<ListingType, int>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value)), 
                            c => c.ToDictionary(k => k.Key, v => v.Value) 
                        ));
            });


        }
    }
}
