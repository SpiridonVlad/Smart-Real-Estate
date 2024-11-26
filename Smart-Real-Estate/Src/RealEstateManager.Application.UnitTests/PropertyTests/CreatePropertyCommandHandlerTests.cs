using Application.Use_Cases.CommandHandlers;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Types;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace RealEstateManager.Application.UnitTests.PropertyTests
{
    public class CreatePropertyCommandHandlerTests
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IValidator<CreatePropertyCommand> validator;

        public CreatePropertyCommandHandlerTests()
        {
            propertyRepository = Substitute.For<IPropertyRepository>();
            mapper = Substitute.For<IMapper>();
            userRepository = Substitute.For<IUserRepository>();
            validator = Substitute.For<IValidator<CreatePropertyCommand>>();
        }

        [Fact]
        public async void Given_CreatePropertyCommandHandler_When_HandleIsCalled_Then_PropertyShouldBeCreated()
        {
            // Arrange
            var property = GenerateProperty();
            var command = new CreatePropertyCommand
            {
                AddressId = property.AddressId,
                Address = property.Address, // Ensure Address is initialized
                ImageId = property.ImageId,
                UserId = property.UserId,
                Type = property.Type,
                Features = property.Features
            };
            propertyRepository.AddAsync(Arg.Any<Property>()).Returns(Result<Guid>.Success(property.Id));
            mapper.Map<Property>(Arg.Any<CreatePropertyCommand>()).Returns(property);
            userRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<User>.Success(property.User));
            validator.ValidateAsync(Arg.Any<CreatePropertyCommand>()).Returns(new ValidationResult());

            // Act
            var handler = new CreatePropertyCommandHandler(propertyRepository, mapper, userRepository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(property.Id);
        }

        [Fact]
        public async void Given_CreatePropertyCommandHandler_When_UserNotFound_Then_FailureShouldBeReturned()
        {
            // Arrange
            var property = GenerateProperty();
            var command = new CreatePropertyCommand
            {
                AddressId = property.AddressId,
                Address = property.Address, // Ensure Address is initialized
                ImageId = property.ImageId,
                UserId = property.UserId,
                Type = property.Type,
                Features = property.Features
            };
            userRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<User>.Failure("UserId does not exist."));
            validator.ValidateAsync(Arg.Any<CreatePropertyCommand>()).Returns(new ValidationResult());

            // Act
            var handler = new CreatePropertyCommandHandler(propertyRepository, mapper, userRepository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("UserId does not exist.");
        }

        [Fact]
        public async void Given_CreatePropertyCommandHandler_When_AddFails_Then_FailureShouldBeReturned()
        {
            // Arrange
            var property = GenerateProperty();
            var command = new CreatePropertyCommand
            {
                AddressId = property.AddressId,
                Address = property.Address, // Ensure Address is initialized
                ImageId = property.ImageId,
                UserId = property.UserId,
                Type = property.Type,
                Features = property.Features
            };
            propertyRepository.AddAsync(Arg.Any<Property>()).Returns(Result<Guid>.Failure("Failed to add property"));
            mapper.Map<Property>(Arg.Any<CreatePropertyCommand>()).Returns(property);
            userRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<User>.Success(property.User));
            validator.ValidateAsync(Arg.Any<CreatePropertyCommand>()).Returns(new ValidationResult());

            // Act
            var handler = new CreatePropertyCommandHandler(propertyRepository, mapper, userRepository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to add property");
        }

        [Fact]
        public async void Given_CreatePropertyCommandHandler_When_ExceptionIsThrown_Then_FailureShouldBeReturned()
        {
            // Arrange
            var property = GenerateProperty();
            var command = new CreatePropertyCommand
            {
                AddressId = property.AddressId,
                Address = property.Address, // Ensure Address is initialized
                ImageId = property.ImageId,
                UserId = property.UserId,
                Type = property.Type,
                Features = property.Features
            };
            propertyRepository.AddAsync(Arg.Any<Property>()).Returns(Task.FromResult(Result<Guid>.Failure("Database error")));
            mapper.Map<Property>(Arg.Any<CreatePropertyCommand>()).Returns(property);
            userRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<User>.Success(property.User));
            validator.ValidateAsync(Arg.Any<CreatePropertyCommand>()).Returns(new ValidationResult());

            // Act
            var handler = new CreatePropertyCommandHandler(propertyRepository, mapper, userRepository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Database error");
        }

        private Property GenerateProperty()
        {
            return new Property
            {
                Id = Guid.NewGuid(),
                AddressId = Guid.NewGuid(),
                Address = new Address
                {
                    Id = Guid.NewGuid(),
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "Anystate",
                    PostalCode = "12345",
                    Country = "USA",
                    AdditionalInfo = "Near the park"
                },
                ImageId = "image123",
                UserId = Guid.NewGuid(),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "testuser",
                    Password = "password",
                    Email = "testuser@example.com",
                    Verified = true,
                    Rating = 4.5m,
                    Type = UserType.Individual,
                    PropertyHistory = new List<Guid> { Guid.NewGuid() }
                },
                Type = PropertyType.Apartment,
                Features = new PropertyFeatures
                {
                    Features = new Dictionary<PropertyFeatureType, int>
                    {
                        { PropertyFeatureType.Balcony, 1 },
                        { PropertyFeatureType.Garage, 1 }
                    }
                }
            };
        }
    }
}