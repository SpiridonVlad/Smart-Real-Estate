using Application.DTOs;
using Application.Use_Cases.CommandHandlers;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Features;
using Domain.Repositories;
using Domain.Types;
using FluentAssertions;
using NSubstitute;

namespace RealEstateManager.Application.UnitTests.UserTests
{
    public class CreateUserCommandHandlerTests
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IPropertyRepository propertyRepository;

        public CreateUserCommandHandlerTests()
        {
            repository = Substitute.For<IUserRepository>();
            mapper = Substitute.For<IMapper>();
            propertyRepository = Substitute.For<IPropertyRepository>();
        }

        [Fact]
        public async void Given_CreateUserCommandHandler_When_HandleIsCalled_Then_UserShouldBeCreated()
        {
            // Arrange
            var user = GenerateUser();
            var command = new CreateUserCommand
            {
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Verified = user.Verified,
                Rating = user.Rating,
                Type = user.Type,
                PropertyHistory = user.PropertyHistory
            };
            repository.AddAsync(Arg.Any<User>()).Returns(Result<Guid>.Success(user.Id));
            mapper.Map<User>(Arg.Any<CreateUserCommand>()).Returns(user);
            propertyRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<Property>.Success(new Property
            {
                Address = new Address
                {
                    Street = "Default Street",
                    City = "Default City",
                    State = "Default State",
                    Country = "Default Country"
                },
                ImageId = "defaultImageId", // Set a default or valid ImageId
                
                Features = new PropertyFeatures() // Assuming PropertyFeatures is a required member
            }));
            // Act
            var handler = new CreateUserCommandHandler(repository, mapper, propertyRepository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(user.Id);
        }

        [Fact]
        public async void Given_CreateUserCommandHandler_When_AddFails_Then_FailureShouldBeReturned()
        {
            // Arrange
            var user = GenerateUser();
            var command = new CreateUserCommand
            {
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Verified = user.Verified,
                Rating = user.Rating,
                Type = user.Type,
                PropertyHistory = null
            };
            repository.AddAsync(Arg.Any<User>()).Returns(Result<Guid>.Failure("Failed to add user"));
            mapper.Map<User>(Arg.Any<CreateUserCommand>()).Returns(user);
            propertyRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<Property>.Success(new Property
            {
                Address = new Address
                {
                    Street = "Default Street",
                    City = "Default City",
                    State = "Default State",
                    Country = "Default Country"
                },
                ImageId = "defaultImageId", // Set a default or valid ImageId
                
                Features = new PropertyFeatures() // Assuming PropertyFeatures is a required member
            }));

            // Act
            var handler = new CreateUserCommandHandler(repository, mapper, propertyRepository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to add user");
        }

        [Fact]
        public async void Given_CreateUserCommandHandler_When_ExceptionIsThrown_Then_FailureShouldBeReturned()
        {
            // Arrange
            var user = GenerateUser();
            var command = new CreateUserCommand
            {
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Verified = user.Verified,
                Rating = user.Rating,
                Type = user.Type,
                PropertyHistory = user.PropertyHistory
            };
            repository.AddAsync(Arg.Any<User>()).Returns(Task.FromResult(Result<Guid>.Failure("Database error")));
            mapper.Map<User>(Arg.Any<CreateUserCommand>()).Returns(user);
            propertyRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<Property>.Success(new Property
            {
                Address = new Address
                {
                    Street = "Default Street",
                    City = "Default City",
                    State = "Default State",
                    Country = "Default Country"
                },
                ImageId = "defaultImageId", // Set a default or valid ImageId
                
                Features = new PropertyFeatures() // Assuming PropertyFeatures is a required member
            }));
            // Act
            var handler = new CreateUserCommandHandler(repository, mapper, propertyRepository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Database error");
        }

        [Fact]
        public async void Given_CreateUserCommandHandler_When_PropertyNotFound_Then_FailureShouldBeReturned()
        {
            // Arrange
            var user = GenerateUser();
            var command = new CreateUserCommand
            {
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Verified = user.Verified,
                Rating = user.Rating,
                Type = user.Type,
                PropertyHistory = [Guid.NewGuid()]
            };
            repository.AddAsync(Arg.Any<User>()).Returns(Result<Guid>.Success(user.Id));
            mapper.Map<User>(Arg.Any<CreateUserCommand>()).Returns(user);
            propertyRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<Property>.Failure("Property not found"));

            // Act
            var handler = new CreateUserCommandHandler(repository, mapper, propertyRepository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Property not found");
        }

        private static User GenerateUser()
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
                PropertyHistory = [Guid.NewGuid()]
            };
        }
    }
}
