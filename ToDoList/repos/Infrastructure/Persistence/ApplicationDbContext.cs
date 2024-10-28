using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ToDoTask> ToDoTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<ToDoTask>(

                    entity =>
                    {
                        entity.ToTable("toDoTasks");
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.Id)
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .ValueGeneratedOnAdd();
                        entity.Property(entity => entity.Title).IsRequired().HasMaxLength(200);
                        entity.Property(entity => entity.Description).IsRequired().HasMaxLength(1000);
                        entity.Property(entity => entity.UserId).IsRequired().HasMaxLength(13);
                        entity.Property(entity => entity.DueDate).IsRequired();
                        entity.Property(entity => entity.IsCompleted).IsRequired();
                    }
                );
        }

    }
    
}
