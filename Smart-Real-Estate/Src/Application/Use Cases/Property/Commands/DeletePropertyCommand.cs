using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Property.Commands
{
    public class DeletePropertyCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
    }
}
