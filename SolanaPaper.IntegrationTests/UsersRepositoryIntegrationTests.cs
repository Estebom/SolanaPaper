using Xunit;
using MongoDB.Driver;
using SolanaPaper.Data.Models;
using SolanaPaper.Data.Repositories;
using Microsoft.Extensions.Options;
using Xunit.Sdk;

namespace SolanaPaper.IntegrationTests
{
    public class UsersRepositoryIntegrationTests
    {

        private readonly TokenRepository tokensRepository;

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

            tokensRepository = new TokenRepository(testMongoDBSettings);

        }

        [Fact]
        public async Task AddAsync_ShouldInsertUserIntoDatabase()
        {
            // Arrange
            //var token = new Tokens() {
                
            //    ContactAddress="123",
            //    Symbol="$penis",
            //    MintAuthority="121451",
            //    Creator="boy",
            //    MintDate=DateTime.Now,
            //    Holders = { }
            
            //};

            //try
            //{
            //    Users users = await usersRepository.GetByUsername("John Doe");
            //    Console.WriteLine(users.Username);
            //}
            //catch (Exception ex) 
            //{
            //    Console.WriteLine(ex.Message);
            //}
            



        }
    }
}
