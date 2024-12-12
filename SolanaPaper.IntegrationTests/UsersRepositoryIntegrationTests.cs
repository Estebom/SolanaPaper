using Xunit;
using MongoDB.Driver;
using SolanaPaper.Data.Models;
using SolanaPaper.Data.Repositories;
using Microsoft.Extensions.Options;

namespace SolanaPaper.IntegrationTests
{
    public class UsersRepositoryIntegrationTests
    {

        private readonly UsersRepository usersRepository;

        public UsersRepositoryIntegrationTests()
        {
            var testMongoDBSettings = Options.Create(new MongoDBSettings
            {
                ConnectionURI = "mongodb+srv://estebom:spiderGuar1463@estebom.y4jnl.mongodb.net/?retryWrites=true&w=majority&appName=Estebom",
                DatabaseName = "SolanaPaper",
                CollectionName1 = "Users",
                CollectionName2 = "Tokens",
                CollectionName3 = "MarketData"

            });

            usersRepository = new UsersRepository(testMongoDBSettings);

        }

        [Fact]
        public async Task AddAsync_ShouldInsertUserIntoDatabase()
        {
            // Arrange
            //Users user = new Users
            //{
            //    Username = "John Doe",
            //    Email = "balls",
            //    Stats = new Stats
            //    {
            //        Gains = 0,
            //        Losses = 0,
            //        Ratio = 0,
            //        RugCount = 0
            //    },
            //    Holdings = new List<string> { "1", "2" }
            //};

            await usersRepository.AddToHoldings("John Doe", "pep");

            Users users = await usersRepository.GetByUsername("John Doe");

            Console.WriteLine(users.Holdings);



        }
    }
}
