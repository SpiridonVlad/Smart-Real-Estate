using Domain.Types;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public UserType Type { get; set; }
        public UserStatus Status { get; set; }
        public bool verified { get; set; }
        public decimal rating { get; set; }
    }
}
