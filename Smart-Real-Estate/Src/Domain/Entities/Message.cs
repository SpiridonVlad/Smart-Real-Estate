namespace Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public required string MessageContent { get; set; }
        public required Guid ChatId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
