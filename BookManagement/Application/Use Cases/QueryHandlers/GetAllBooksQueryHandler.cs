using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetAllBooksQueryHandler(IBookRepository repository, IMapper mapper) : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IBookRepository repository = repository;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await repository.GetAllAsync();
            return mapper.Map<IEnumerable<BookDto>>(books);
        }
    }
}
