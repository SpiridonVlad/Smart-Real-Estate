using Application.Use_Cases.Users.CommandHandlers;
using Application.Use_Cases.Users.Commands;
using Domain.Common;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;


namespace RealEstateManager.Application.UnitTests.UserTests
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly IUserRepository repository;

        public DeleteUserCommandHandlerTests()
        {
            repository = Substitute.For<IUserRepository>();
        }

        [Fact]
        public async void Given_DeleteUserCommandHandler_When_HandleIsCalled_Then_UserShouldBeDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand { Id = userId };
            repository.DeleteAsync(Arg.Any<Guid>()).Returns(Result<object>.Success(""));

            // Act
            var handler = new DeleteUserCommandHandler(repository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeNull();
        }

        [Fact]
        public async void Given_DeleteUserCommandHandler_When_DeleteFails_Then_FailureShouldBeReturned()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand { Id = userId };
            repository.DeleteAsync(Arg.Any<Guid>()).Returns(Result<object>.Failure("Failed to delete user"));

            // Act
            var handler = new DeleteUserCommandHandler(repository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to delete user");
        }

        [Fact]
        public async void Given_DeleteUserCommandHandler_When_ExceptionIsThrown_Then_FailureShouldBeReturned()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand { Id = userId };
            repository.DeleteAsync(Arg.Any<Guid>()).Returns(Task.FromResult(Result<object>.Failure("Database error")));

            // Act
            var handler = new DeleteUserCommandHandler(repository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Database error");
        }
    }
}

