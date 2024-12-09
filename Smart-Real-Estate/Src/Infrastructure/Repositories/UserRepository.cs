using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context, IConfiguration configuration) : IUserRepository
    {
        private readonly ApplicationDbContext context = context;
        private readonly IConfiguration configuration = configuration;

        public async Task<Result<Guid>> AddAsync(User user)
        {
            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(user.Id);
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
                var user = await context.Users.FindAsync(id);
                if (user == null)
                {
                    return Result<object>.Failure("User not found");
                }

                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Result<object>.Success("Deleted succesfully");
            }
            catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<User>>> GetAllAsync()
        {
            try
            {
                var users = await context.Users.ToListAsync();
                return Result<IEnumerable<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<User>>.Failure(ex.Message);
            }
        }

        public async Task<Result<User>> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await context.Users.FindAsync(id);
                if (user == null)
                {
                    return Result<User>.Failure("User not found");
                }

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure(ex.Message);
            }
        }

        public async Task<Result<object>> UpdateAsync(User user)
        {
            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return Result<object>.Success("Updated succesfully");
            }
            catch (Exception ex)
            {
                return Result<object>.Failure(ex.Message);
            }
        }
        public async Task<Result<IEnumerable<User>>> GetPaginatedAsync(int page, int pageSize)
        {
            try
            {
                var users = await context.Users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return Result<IEnumerable<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<User>>.Failure(ex.Message);
            }
        }

        public async Task<Result<string>> Login(User user)
        {
            var existingUser = await context.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser == null || existingUser.Password != user.Password)
                return Result<string>.Failure("Invalid email or password");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Result<string>.Success(tokenHandler.WriteToken(token));
        }

        public async Task<Result<Guid>> Register(User user, CancellationToken cancellationToken)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(user.Id);
        }


    }
}
