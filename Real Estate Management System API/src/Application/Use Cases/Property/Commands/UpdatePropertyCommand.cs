using MediatR;

namespace Application.Use_Cases.Commands
{
    public class UpdatePropertyCommand : CreatePropertyCommand, IRequest
    {
        public Guid Id { get; set; }
    }
}
