using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Authentication
{
    public class ConfirmEmailCommand() : IRequest<Result<string>>
    {
        public required string Token { get; set; }
    }
}
