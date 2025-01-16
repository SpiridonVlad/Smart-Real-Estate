namespace Domain.Entities.Messages
{
    public class Chat
    {
        public required Guid ChatId { get; set; }
        public required Guid Participant1Id { get; set; }
        public required Guid Participant2Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
    }
}
