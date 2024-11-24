using Application.DTOs;
using Application.Use_Cases.Commands;
using Application.Use_Cases.Users.CommandHandlers;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Types;
using FluentAssertions;
using NSubstitute;


namespace RealEstateManager.Application.UnitTests.UserTests
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UpdateUserCommandHandlerTests()
        {
            repository = Substitute.For<IUserRepository>();
            mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async void Given_UpdateUserCommandHandler_When_HandleIsCalled_Then_UserShouldBeUpdated()
        {
            // Arrange
            var user = GenerateUser();
            var command = new UpdateUserCommand
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Verified = user.Verified,
                Rating = user.Rating,
                Type = user.Type,
                PropertyHistory = user.PropertyHistory
            };
            repository.UpdateAsync(Arg.Any<User>()).Returns(Result<object>.Success(null));
            mapper.Map<User>(Arg.Any<UpdateUserCommand>()).Returns(user);

            // Act
            var handler = new UpdateUserCommandHandler(repository, mapper);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be("User updated successfully");
        }

        [Fact]
        public async void Given_UpdateUserCommandHandler_When_UpdateFails_Then_FailureShouldBeReturned()
        {
            // Arrange
            var user = GenerateUser();
            var command = new UpdateUserCommand
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Verified = user.Verified,
                Rating = user.Rating,
                Type = user.Type,
                PropertyHistory = user.PropertyHistory
            };
            repository.UpdateAsync(Arg.Any<User>()).Returns(Result<object>.Failure("Failed to update user"));
            mapper.Map<User>(Arg.Any<UpdateUserCommand>()).Returns(user);

            // Act
            var handler = new UpdateUserCommandHandler(repository, mapper);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to update user");
        }

        [Fact]
        public async void Given_UpdateUserCommandHandler_When_ExceptionIsThrown_Then_FailureShouldBeReturned()
        {
            // Arrange
            var user = GenerateUser();
            var command = new UpdateUserCommand
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Verified = user.Verified,
                Rating = user.Rating,
                Type = user.Type,
                PropertyHistory = user.PropertyHistory
            };
            repository.UpdateAsync(Arg.Any<User>()).Returns(Task.FromResult(Result<object>.Failure("Database error")));
            mapper.Map<User>(Arg.Any<UpdateUserCommand>()).Returns(user);

            // Act
            var handler = new UpdateUserCommandHandler(repository, mapper);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Database error");
        }

        private User GenerateUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Password = "password",
                Email = "testuser@example.com",
                Verified = true,
                Rating = 4.5m,
                Type = UserType.Individual,
                PropertyHistory = new List<Guid> { Guid.NewGuid() }
            };
        }

        private UserDto GenerateUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Verified = user.Verified,
                Rating = user.Rating,
                Type = user.Type,
                PropertyHistory = user.PropertyHistory
            };
        }
    }
}

