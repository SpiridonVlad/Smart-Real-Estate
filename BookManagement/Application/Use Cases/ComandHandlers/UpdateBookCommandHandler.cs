using Application.Use_Cases.Commands;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.ComandHandlers
{

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand>
    {
        private readonly IBookRepository repository;

        public UpdateBookCommandHandler(IBookRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await repository.GetByIdAsync(request.Id);

            if (!string.IsNullOrEmpty(request.Title))
            {
                book.Title = request.Title;
            }
            if (!string.IsNullOrEmpty(request.Author))
            {
                book.Author = request.Author;
            }
            if (!string.IsNullOrEmpty(request.ISBN))
            {
                book.ISBN = request.ISBN;
            }
            if (request.PublicationDate.HasValue)
            {
                book.PublicationDate = request.PublicationDate.Value;
            }

            await repository.UpdateAsync(book);

        }

    }
}
