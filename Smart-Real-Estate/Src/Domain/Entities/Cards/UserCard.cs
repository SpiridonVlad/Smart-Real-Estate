using Domain.Types;

namespace Domain.Entities.Cards
{
    public class UserCard
    {
        public required string Username { get; set; }
        public bool Verified { get; set; }
        public decimal Rating { get; set; }
        public UserType Type { get; set; }
    }
}
