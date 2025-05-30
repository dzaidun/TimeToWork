using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using TimeToWork.Data;
using TimeToWork.Models;
using Xunit;

namespace TimeToWork.Tests
{
    public class ClientTests
    {
        private TimeToWorkContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TimeToWorkContext>()
                .UseInMemoryDatabase(databaseName: "TestClientDb")
                .Options;

            return new TimeToWorkContext(options);
        }

        [Fact]
        public async Task CanCreateClientInDatabase()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var client = new Client
            {
                FirstName = "Олег",
                LastName = "Коваленко",
                PhoneNumber = "380931234567"
            };

            // Act
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            // Assert
            var savedClient = await context.Clients.FirstOrDefaultAsync(c => c.PhoneNumber == "380931234567");
            Assert.NotNull(savedClient);
            Assert.Equal("Олег", savedClient.FirstName);
            Assert.Equal("Коваленко", savedClient.LastName);
        }
    }
}
