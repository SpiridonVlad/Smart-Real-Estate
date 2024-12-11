using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class AddressRepository(ApplicationDbContext context) : IAddressRepository
    {
        private readonly ApplicationDbContext context = context;

        public async Task<Result<Guid>> AddAsync(Address address)
        {
            try
            {
                await context.Addresses.AddAsync(address);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(address.Id);
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
                var address = await context.Addresses.FindAsync(id);
                if (address == null)
                {
                    return Result<object>.Failure("Address not found");
                }
                context.Addresses.Remove(address);
                await context.SaveChangesAsync();
                return Result<object>.Success(address);
            }
            catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }

        public async Task<Result<Address>> GetByIdAsync(Guid id)
        {
            try
            {
                var address = await context.Addresses.FindAsync(id);
                if (address == null)
                {
                    return Result<Address>.Failure("Address not found");
                }
                return Result<Address>.Success(address);
            }
            catch (Exception ex)
            {
                return Result<Address>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Address>>> GetPaginatedAsync(int page, int pageSize)
        {
            try
            {
                var addresses = await context.Addresses
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
                return Result<IEnumerable<Address>>.Success(addresses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Address>>.Failure(ex.Message);
            }
        }

        public async Task<Result<object>> UpdateAsync(Address address)
        {
            try
            {
                context.Addresses.Update(address);
                await context.SaveChangesAsync();
                return Result<object>.Success(address);
            }
            catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }
    }
}
