using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly ApplicationDbContext context;

        public ListingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> AddAsync(Listing listing)
        {
            try
            {
                await context.Listings.AddAsync(listing);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(listing.Id);
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
                var listing = await context.Listings.FindAsync(id);
                if (listing == null)
                {
                    return Result<object>.Failure("Listing not found");
                }

                context.Listings.Remove(listing);
                await context.SaveChangesAsync();
                return Result<object>.Success(listing);
            }
            catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Listing>>> GetAllAsync()
        {
            try
            {
                var listings = await context.Listings.ToListAsync();
                return Result<IEnumerable<Listing>>.Success(listings);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Listing>>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Listing>>> GetPaginatedAsync(int page, int pageSize)
        {
            try
            {
                var listings = await context.Listings
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return Result<IEnumerable<Listing>>.Success(listings);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Listing>>.Failure(ex.Message);
            }
        }

        public async Task<Result<Listing>> GetByIdAsync(Guid id)
        {
            try
            {
                var listing = await context.Listings.FindAsync(id);
                if (listing == null)
                {
                    return Result<Listing>.Failure("Listing not found");
                }

                return Result<Listing>.Success(listing);
            }
            catch (Exception ex)
            {
                return Result<Listing>.Failure(ex.Message);
            }
        }

        public async Task<Result<object>> UpdateAsync(Listing listing)
        {
            try
            {
                context.Listings.Update(listing);
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
