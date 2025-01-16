using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class PropertyRepository(ApplicationDbContext context) : IPropertyRepository
    {
        private readonly ApplicationDbContext context = context;

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

        public async Task<Result<object>> DeleteUsersPropertys(Guid userId)
        {
            try
            {
                var properties = await context.Properties.Where(p => p.UserId == userId).ToListAsync();
                if (properties == null)
                {
                    return Result<object>.Failure("Properties not found");
                }
                context.Properties.RemoveRange(properties);
                await context.SaveChangesAsync();

                return Result<object>.Success(properties);
            }
            catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Property>>> GetPaginatedAsync(
            int page, 
            int pageSize,
            Expression<Func<Property, bool>>? filter = null)
        {
            try
            {
                IQueryable<Property> query = context.Properties;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                var properties = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Result<IEnumerable<Property>>.Success(properties);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Property>>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Property>>> GetAllForUserAsync(Guid userId)
        {
            try
            {
                var properties = await context.Properties.Where(p => p.UserId == userId).ToListAsync();
                Console.WriteLine(properties);
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
                return Result<object>.Success("Updated succesfully");
            }
            catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }
    }
}
