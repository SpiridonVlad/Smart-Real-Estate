using Domain.Entities;
using Domain.Types;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public DbSet<Address> Addresses { get; set; }

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

                entity.Property(e => e.Street)
                    .IsRequired();

                entity.Property(e => e.City)
                    .IsRequired();

                entity.Property(e => e.State)
                    .IsRequired();

                entity.Property(e => e.PostalCode)
                    .IsRequired();

                entity.Property(e => e.Country)
                    .IsRequired();

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

                entity.Property(e => e.AdressId)
                    .HasColumnType("uuid")
                    .IsRequired();

                entity.HasOne(e => e.Address)
                    .WithMany()
                    .HasForeignKey(e => e.AdressId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ImageId)
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .HasColumnType("uuid")
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Type)
                    .HasConversion<string>()
                    .IsRequired();

                entity.OwnsOne(e => e.Features, features =>
                {
                    features.Property(f => f.Features)
                        .HasConversion(
                            v => string.Join(',', v.Select(kv => $"{kv.Key}:{kv.Value}")),
                            v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(s => s.Split(new[] { ':' }, StringSplitOptions.None))
                                  .Where(parts => parts.Length == 2)
                                  .ToDictionary(
                                      kv => Enum.Parse<PropertyFeatureType>(kv[0]),
                                      kv => int.Parse(kv[1])
                                  )
                        )
                        .Metadata
                        .SetValueComparer(new ValueComparer<Dictionary<PropertyFeatureType, int>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToDictionary(kv => kv.Key, kv => kv.Value)
                        ));
                });
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
                    .HasColumnType("int");

                entity.Property(e => e.PublicationDate)
                    .IsRequired();

                entity.Property(e => e.Properties)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<List<ListingAssetss>>(v)
                    )
                    .HasColumnType("jsonb"); 
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

                entity.Property(e => e.Type)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(e => e.PropertyHistory)
                    .HasColumnType("jsonb") 
                    .IsRequired(false); 
            });
        }
    }
}