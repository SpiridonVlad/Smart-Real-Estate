﻿using Domain.Common;
using Domain.Types;
using MediatR;

namespace Application.Use_Cases.Commands
{
    public class UpdateUserCommand :  IRequest<Result<string>>
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public bool Verified { get; set; }
        public decimal Rating { get; set; }
        public UserType Type { get; set; }
        public List<Guid>? PropertyHistory { get; set; }

    }
}
