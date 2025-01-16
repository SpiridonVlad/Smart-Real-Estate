﻿using Application.Use_Cases.Users.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Users.CommandHandlers
{
    public class AddPropertyToHistoryCommandHandler(IUserRepository repository, IMapper mapper, IPropertyRepository propertyRepository) : IRequestHandler<AddPropertyToHistoryCommand, Result<string>>
    {
        private readonly IUserRepository repository = repository;
        private readonly IPropertyRepository propertyRepository = propertyRepository;

        public async Task<Result<string>> Handle(AddPropertyToHistoryCommand request, CancellationToken cancellationToken)
        {
            var userResult = await repository.GetByIdAsync(request.UserId);
            if (!userResult.IsSuccess)
            {
                return Result<string>.Failure("User not found");
            }

            var propertyResult = await propertyRepository.GetByIdAsync(request.PropertyId);
            if (!propertyResult.IsSuccess)
            {
                return Result<string>.Failure("Property not found");
            }

            var user = userResult.Data;
            if (user.PropertyHistory == null)
            {
                user.PropertyHistory = [];
            }
            user.PropertyHistory.Add(request.PropertyId);

            var updateResult = await repository.UpdateAsync(user);
            if (updateResult.IsSuccess)
            {
                return Result<string>.Success("Property added to user's history successfully");
            }

            return Result<string>.Failure(updateResult.ErrorMessage);

        }
    }
}
