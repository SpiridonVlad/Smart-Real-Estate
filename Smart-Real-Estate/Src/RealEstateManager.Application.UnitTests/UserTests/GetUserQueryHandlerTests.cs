using Application.DTOs;
using Application.Use_Cases.Queries;
using Application.Use_Cases.QueryHandlers;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Types;
using FluentAssertions;
using NSubstitute;


namespace RealEstateManager.Application.UnitTests.UserTests
{
    public class GetUserQueryHandlerTests
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public GetUserQueryHandlerTests()
        {
            repository = Substitute.For<IUserRepository>();
            mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async void Given_GetUserByIdQueryHandler_When_HandleIsCalled_Then_UserShouldBeReturned()
        {
            // Arrange
            var user = GenerateUser();
            var userDto = GenerateUserDto(user);
            repository.GetByIdAsync(user.Id).Returns(Result<User>.Success(user));
            mapper.Map<UserDto>(user).Returns(userDto);
            var query = new GetUserByIdQuery { Id = user.Id };

            // Act
            var handler = new GetUserByIdQueryHandler(mapper, repository);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async void Given_GetUserByIdQueryHandler_When_UserNotFound_Then_FailureShouldBeReturned()
        {
            // Arrange
            var userId = Guid.NewGuid();
            repository.GetByIdAsync(userId).Returns(Result<User>.Failure("User not found"));
            var query = new GetUserByIdQuery { Id = userId };

            // Act
            var handler = new GetUserByIdQueryHandler(mapper, repository);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("User not found");
        }


        [Fact]
        public async void Given_GetUserByIdQueryHandler_When_ExceptionIsThrown_Then_FailureShouldBeReturned()
        {
            // Arrange
            var userId = Guid.NewGuid();
            repository.GetByIdAsync(userId).Returns(Task.FromException<Result<User>>(new Exception("Database error")));
            var query = new GetUserByIdQuery { Id = userId };

            // Act
            var handler = new GetUserByIdQueryHandler(mapper, repository);
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
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
                PropertyHistory = null
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
