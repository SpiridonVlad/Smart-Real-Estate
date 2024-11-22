using Domain.Types;

namespace Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public bool Verified { get; set; }
        public decimal Rating { get; set; }
        public UserType Type { get; set; }
        public List<Guid>? PropertyHistory { get; set; }

    }
}
