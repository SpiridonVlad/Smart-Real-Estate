using MediatR;

namespace Application.Use_Cases.Commands
{
    public class UpdateBookCommand: IRequest
    {
        public Guid Id { get; set; }  
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public DateTime? PublicationDate { get; set; }
    }
}
