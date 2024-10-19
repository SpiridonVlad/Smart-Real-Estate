using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetBookByIdQuery : IRequest<BookDto>
    {
        public Guid Id { get; set; }
    }
}
