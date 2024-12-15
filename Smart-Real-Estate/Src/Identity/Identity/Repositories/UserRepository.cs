using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace Identity.Repositories
{
    public class UserRepository(UsersDbContext context, IConfiguration configuration) : IUserRepository
    {
        private readonly UsersDbContext context = context;
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
            if (context == null)
                return Result<User>.Failure("Database context is not initialized.");
           
            try
            {
                var user = await context.Users.FirstAsync(u => u.Id == id);

                if (user == null)
                {
                    return Result<User>.Failure("User not found");
                }

                return Result<User>.Success(user);
            }
            catch (DbUpdateException ex)
            {
                return Result<User>.Failure("Database update error: " + ex.Message);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure("An unexpected error occurred: " + ex.Message);
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

        public async Task<Result<IEnumerable<User>>> GetPaginatedAsync(
            int page,
            int pageSize,
            Expression<Func<User, bool>>? filter = null)
        {
            try
            {
                IQueryable<User> query = context.Users;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                var users = await query
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
            Console.WriteLine("Existing user: " + existingUser.Password);
            if (existingUser == null)
                return Result<string>.Failure("Invalid email or password");
            if (user.Password != existingUser.Password && !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
                return Result<string>.Failure("Invalid email or password");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
        {
            new Claim("userId", existingUser.Id.ToString()),
            new Claim(ClaimTypes.Role, existingUser.Type.ToString())
        }),
                Expires = DateTime.UtcNow.AddDays(7),
                Audience = configuration["Jwt:Audience"],
                Issuer = configuration["Jwt:Issuer"],
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

        public string GenerateEmailConfirmationToken(string email, string username, string password)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var claims = new[]
            {
        new Claim("Email", email),
        new Claim("Username", username),
        new Claim("Password", hashedPassword) 
    };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
