﻿using Application.Use_Cases.CommandHandlers;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Types;
using FluentAssertions;
using NSubstitute;

namespace RealEstateManager.Application.UnitTests.PropertyTests
{
    public class UpdatePropertyCommandHandlerTests
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IMapper mapper;

        public UpdatePropertyCommandHandlerTests()
        {
            propertyRepository = Substitute.For<IPropertyRepository>();
            mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task Given_UpdatePropertyCommandHandler_When_HandleIsCalled_Then_PropertyShouldBeUpdated()
        {
            // Arrange
            var property = GenerateProperty();
            var command = new UpdatePropertyCommand
            {
                Id = property.Id,
                AddressId = property.AddressId,
                Address = property.Address,
                ImageId = property.ImageId,
                UserId = property.UserId,
                Type = property.Type,
                Features = property.Features
            };
            propertyRepository.UpdateAsync(Arg.Any<Property>()).Returns(Result<object>.Success(null));
            mapper.Map(Arg.Any<UpdatePropertyCommand>(), Arg.Any<Property>()).Returns(property);

            // Act
            var handler = new UpdatePropertyCommandHandler(propertyRepository, mapper);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be("Property updated successfully");
        }

        [Fact]
        public async Task Given_UpdatePropertyCommandHandler_When_PropertyNotFound_Then_FailureShouldBeReturned()
        {
            // Arrange
            var command = new UpdatePropertyCommand
            {
                Id = Guid.NewGuid(),
                AddressId = Guid.NewGuid(),
                Address = new Address
                {
                    Id = Guid.NewGuid(),
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "Anystate",
                    PostalCode = "123456",
                    Country = "USA",
                    AdditionalInfo = "Near the park"
                },
                ImageId = "image123",
                UserId = Guid.NewGuid(),
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
            propertyRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<Property>.Failure("Property not found."));

            // Act
            var handler = new UpdatePropertyCommandHandler(propertyRepository, mapper);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Property not found.");
        }

        [Fact]
        public async Task Given_UpdatePropertyCommandHandler_When_UpdateFails_Then_FailureShouldBeReturned()
        {
            // Arrange
            var property = GenerateProperty();
            var command = new UpdatePropertyCommand
            {
                Id = property.Id,
                AddressId = property.AddressId,
                Address = property.Address,
                ImageId = property.ImageId,
                UserId = property.UserId,
                Type = property.Type,
                Features = property.Features
            };
            propertyRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<Property>.Success(property));
            propertyRepository.UpdateAsync(Arg.Any<Property>()).Returns(Result<object>.Failure("Failed to update property"));
            mapper.Map(Arg.Any<UpdatePropertyCommand>(), Arg.Any<Property>()).Returns(property);

            // Act
            var handler = new UpdatePropertyCommandHandler(propertyRepository, mapper);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to update property");
        }

        [Fact]
        public async Task Given_UpdatePropertyCommandHandler_When_ExceptionIsThrown_Then_FailureShouldBeReturned()
        {
            // Arrange
            var property = GenerateProperty();
            var command = new UpdatePropertyCommand
            {
                Id = property.Id,
                AddressId = property.AddressId,
                Address = property.Address,
                ImageId = property.ImageId,
                UserId = property.UserId,
                Type = property.Type,
                Features = property.Features
                
            };
            propertyRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(Result<Property>.Success(property));
            propertyRepository.UpdateAsync(Arg.Any<Property>()).Returns(Task.FromException<Result<object>>(new Exception("Database error")));
            mapper.Map(Arg.Any<UpdatePropertyCommand>(), Arg.Any<Property>()).Returns(property);

            // Act
            var handler = new UpdatePropertyCommandHandler(propertyRepository, mapper);
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
                    PostalCode = "123456",
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
