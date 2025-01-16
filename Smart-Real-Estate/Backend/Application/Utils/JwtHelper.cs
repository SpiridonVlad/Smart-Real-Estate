using Domain.Common;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Utils
{
    public class JwtHelper
    {
        public static Result<Guid> GetUserIdFromJwt(HttpContext httpContext)
        {
            try
            {
                var authHeader = httpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Result<Guid>.Failure("JWT nu a fost găsit în antet.");
                }

                var token = authHeader.Substring("Bearer ".Length);

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");

                if (userIdClaim == null)
                {
                    return Result<Guid>.Failure("Claim-ul userId nu există în token.");
                }

                if (!Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Result<Guid>.Failure("Format invalid pentru userId.");
                }

                return Result<Guid>.Success(userId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Eroare la procesarea JWT: {ex.Message}");
            }
        }
    }
}
