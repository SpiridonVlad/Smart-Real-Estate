using Application.DTOs;
using Application.Use_Cases.Property.QueryHandlers;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Features;
using Domain.Repositories;
using Domain.Types;
using FluentAssertions;
using NSubstitute;


namespace RealEstateManager.Application.UnitTests.PropertyTests
{
    public class GetPropertyQueryHandlerTests
    {
        private readonly IPropertyRepository repository;
        private readonly IMapper mapper;

        public GetPropertyQueryHandlerTests()
        {
            repository = Substitute.For<IPropertyRepository>();
            mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async void Given_GetPropertyByIdQueryHandler_When_HandleIsCalled_Then_PropertyShouldBeReturned()
        {
            // Arrange
            var property = GenerateProperty();
            var propertyDto = GeneratePropertyDto(property);
            repository.GetByIdAsync(property.Id).Returns(Result<Property>.Success(property));
            mapper.Map<PropertyDto>(property).Returns(propertyDto);
            var query = new GetPropertyByIdQuery { Id = property.Id };

            // Act
            var handler = new GetPropertyByIdQueryHandler(mapper, repository);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(propertyDto);
        }

        [Fact]
        public async void Given_GetPropertyByIdQueryHandler_When_PropertyNotFound_Then_FailureShouldBeReturned()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            repository.GetByIdAsync(propertyId).Returns(Result<Property>.Failure("Property not found"));
            var query = new GetPropertyByIdQuery { Id = propertyId };

            // Act
            var handler = new GetPropertyByIdQueryHandler(mapper, repository);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Property not found");
        }

        [Fact]
        public async void Given_GetPropertyByIdQueryHandler_When_ExceptionIsThrown_Then_FailureShouldBeReturned()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            repository.GetByIdAsync(propertyId).Returns(Task.FromException<Result<Property>>(new Exception("Database error")));
            var query = new GetPropertyByIdQuery { Id = propertyId };

            // Act
            var handler = new GetPropertyByIdQueryHandler(mapper, repository);
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
        }

        private static Property GenerateProperty()
        {
            return new Property
            {
                Id = Guid.NewGuid(),
                AddressId = Guid.NewGuid(),
                Address = new Address
                {
                    Id = Guid.NewGuid(),
                    Street = "123 Main St",
                    City = "Sample City",
                    State = "Sample State",
                    Country = "Sample Country"
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
                    PropertyHistory = [Guid.NewGuid()]
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


        private static PropertyDto GeneratePropertyDto(Property property)
        {
            return new PropertyDto
            {
                Id = property.Id,
                AddressId = property.AddressId,
                ImageId = property.ImageId,
                UserId = property.UserId,
                User = property.User,
                Type = property.Type,
                Features = property.Features
            };
        }
    }
}

