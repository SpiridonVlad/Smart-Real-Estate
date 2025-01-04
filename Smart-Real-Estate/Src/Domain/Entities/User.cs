using Domain.Types;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public bool Verified { get; set; }
        public decimal Rating { get; set; }
        public UserType Type { get; set; }
        public UserStatus Status { get; set; }
        public List<Guid>? PropertyHistory { get; set; } 
        public List<Guid>? PropertyWaitingList { get; set; } = [];
        public List<Guid>? ChatId { get; set; } = [];
    }
}
