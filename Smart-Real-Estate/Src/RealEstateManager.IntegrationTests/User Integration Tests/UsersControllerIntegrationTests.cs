using Application.Use_Cases.Commands;
using Application.DTOs;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

using Microsoft.VisualStudio.TestPlatform.TestHost;
using Domain.Types;

namespace RealEstateManager.IntegrationTests.User_Integration_Tests
{
    public class UsersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly ApplicationDbContext dbContext;

        private string BaseUrl = "/api/v1/user";
        public UsersControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<ApplicationDbContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });

            var scope = this.factory.Services.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        [Fact]
        public void GivenUsers_WhenGetAllIsCalled_ThenReturnsTheRightContentType()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var response = client.GetAsync($"{BaseUrl}?page=1&pageSize=10");

            // assert
            response.Result.EnsureSuccessStatusCode();
            response.Result.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
        }

        [Fact]
        public void GivenExistingUsers_WhenGetAllIsCalled_ThenReturnsTheRightUsers()
        {
            // arrange
            var client = factory.CreateClient();
            CreateSUT();

            // act
            var response = client.GetAsync($"{BaseUrl}?page=1&pageSize=10");

            // assert
            response.Result.EnsureSuccessStatusCode();
            var users = response.Result.Content.ReadAsStringAsync().Result;
            users.Should().Contain("testuser");
        }

        [Fact]
        public async void GivenValidUser_WhenCreatedIsCalled_Then_ShouldAddToDatabaseTheUser()
        {
            // Arrange
            var client = factory.CreateClient();

            var command = new CreateUserCommand
            {
                Username = "testuser",
                Password = "password",
                Email = "testuser@example.com",
                Verified = true,
                Rating = 4.5m,
                Type = UserType.Individual,
                PropertyHistory = new List<Guid>()
            };

            // Act
            await client.PostAsJsonAsync(BaseUrl, command);

            // Assert
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
            user.Should().NotBeNull();
        }

        [Fact]
        public async void GivenValidUserId_WhenGetUserByIdIsCalled_Then_ShouldReturnTheUser()
        {
            // Arrange
            var client = factory.CreateClient();
            var user = CreateSUT();

            // Act
            var response = await client.GetAsync($"{BaseUrl}/{user.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedUser = await response.Content.ReadFromJsonAsync<UserDto>();
            returnedUser.Should().NotBeNull();
            returnedUser.Username.Should().Be(user.Username);
        }

        [Fact]
        public async void GivenValidUserId_WhenDeleteUserIsCalled_Then_ShouldRemoveFromDatabaseTheUser()
        {
            // Arrange
            var client = factory.CreateClient();
            var user = CreateSUT();

            // Act
            var response = await client.DeleteAsync($"{BaseUrl}/{user.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var deletedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            deletedUser.Should().BeNull();
        }

        [Fact]
        public async void GivenValidUser_WhenUpdateUserIsCalled_Then_ShouldUpdateTheUserInDatabase()
        {
            // Arrange
            var client = factory.CreateClient();
            var user = CreateSUT();

            var command = new UpdateUserCommand
            {
                Id = user.Id,
                Username = "updateduser",
                Password = "newpassword",
                Email = "updateduser@example.com",
                Verified = true,
                Rating = 4.5m,
                Type = UserType.Individual,
                PropertyHistory = new List<Guid>()
            };

            // Act
            var response = await client.PutAsJsonAsync($"{BaseUrl}/{user.Id}", command);

            // Assert
            response.EnsureSuccessStatusCode();
            var updatedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            updatedUser.Should().NotBeNull();
            updatedUser.Username.Should().Be("updateduser");
        }

        public void Dispose()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        private User CreateSUT()
        {
            var user = new User
            {
                Username = "testuser",
                Password = "password",
                Email = "testuser@example.com",
                Verified = true,
                Rating = 4.5m,
                Type = UserType.Individual,
                PropertyHistory = new List<Guid>()
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user;
        }
    }
}

