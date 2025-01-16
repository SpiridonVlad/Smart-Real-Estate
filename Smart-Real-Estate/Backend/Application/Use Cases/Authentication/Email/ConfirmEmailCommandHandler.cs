using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.Use_Cases.Authentication
{
    public class ConfirmEmailCommandHandler(IUserRepository userRepository, IConfiguration configuration) : IRequestHandler<ConfirmEmailCommand, Result<string>>
    {
        private readonly IUserRepository userRepository = userRepository;
        private readonly string jwtSecretKey = configuration["Jwt:Key"];
        private readonly IConfiguration configuration = configuration;

        public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSecretKey);
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true
                };

                var claims = handler.ValidateToken(request.Token, tokenValidationParameters, out var validatedToken);

                var jwtToken = handler.ReadJwtToken(request.Token);

                var email = jwtToken.Claims.First(claim => claim.Type == "Email").Value;

                var username = claims.FindFirst("Username")?.Value;

                var hashedPassword = claims.FindFirst("Password")?.Value;

                if (email == null || username == null || hashedPassword == null)
                {
                    return Result<string>.Failure("Invalid token data.");
                }

                var user = new User
                {
                    Email = email,
                    Username = username,
                    Password = hashedPassword
                };

                var registrationResult = await userRepository.Register(user, cancellationToken);
                if (registrationResult.IsSuccess)
                {
                    var loginResult = await userRepository.Login(user);
                    if (loginResult.IsSuccess)
                    {
                        return Result<string>.Success(loginResult.Data);
                    }
                    else
                    {
                        return Result<string>.Failure("Failed to login user.");
                    }
                }
                else
                {
                    return Result<string>.Failure("Failed to register user.");
                }
            }
            catch (SecurityTokenException ex)
            {
                return Result<string>.Failure($"Token validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Error: {ex.Message}");
            }
        }

    }


}
