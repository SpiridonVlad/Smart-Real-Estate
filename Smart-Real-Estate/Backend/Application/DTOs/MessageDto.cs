namespace Application.DTOs
{
    public class MessageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public required string MessageContent { get; set; }
        public required string ChatId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
