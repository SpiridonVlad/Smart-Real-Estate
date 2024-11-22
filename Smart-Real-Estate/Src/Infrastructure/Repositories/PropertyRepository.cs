using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDbContext context;

        public PropertyRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> AddAsync(Property property)
        {
            try
            {
                await context.Properties.AddAsync(property);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(property.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }

        public async Task<Result<object>> DeleteAsync(Guid id)
        {
            try
            {
                var property = await context.Properties.FindAsync(id);
                if (property == null)
                {
                    return Result<object>.Failure("Property not found");
                }

                context.Properties.Remove(property);
                await context.SaveChangesAsync();
                return Result<object>.Success(property);
            }
            catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Property>>> GetAllAsync()
        {
            try
            {
                var properties = await context.Properties.ToListAsync();
                return Result<IEnumerable<Property>>.Success(properties);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Property>>.Failure(ex.Message);
            }
        }

        public async Task<Result<Property>> GetByIdAsync(Guid id)
        {
            try
            {
                var property = await context.Properties.FindAsync(id);
                if (property == null)
                {
                    return Result<Property>.Failure("Property not found");
                }

                return Result<Property>.Success(property);
            }
            catch (Exception ex)
            {
                return Result<Property>.Failure(ex.Message);
            }
        }

        public async Task<Result<object>> UpdateAsync(Property property)
        {
            try
            {
                context.Properties.Update(property);
                await context.SaveChangesAsync();
                return Result<object>.Success(null);
            }
            catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }
    }
}
