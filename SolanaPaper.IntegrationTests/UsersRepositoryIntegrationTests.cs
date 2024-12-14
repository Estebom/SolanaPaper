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

        private readonly MarketDataRepository marketRepository;

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

            marketRepository = new MarketDataRepository(testMongoDBSettings);

        }

        [Fact]
        public async Task AddAsync_ShouldInsertUserIntoDatabase()
        {
            ////// Arrange
            //Tokens token = new Tokens()
            //{

            //    ContactAddress = "1234",
            //    Symbol = "$penis",
            //    MintAuthority = "121451",
            //    Creator = "boy",
            //    MintDate = DateTime.Now,
            //    Holders = new List<Holder>()
            //    {
            //        new Holder()
            //        {
            //            WalletKey = "123",
            //            TokensHeld = 100
            //        }
            //    },
            //    SocialLinks = new Socials()
            //    {
            //        TwitterLink = "https://twitter.com",
            //        TelegramLink = "https://t.me",
            //        WebsiteLink = "https://website.com"
            //    }

            //};

            //await tokensRepository.Create(token);

            ///NEXT DO GET BY SYMBOL
            //List<Tokens> tokens = await tokensRepository.GetBySymbol("$penis");

            //foreach (var t in tokens)
            //{
            //    Console.WriteLine(t.Symbol);
            //}
            //Console.WriteLine(tokens.Count);

            //await tokensRepository.UpdateSocials("1234", new Socials() { TelegramLink="boy", TwitterLink="eat", WebsiteLink="myballs"});

            //Tokens token = await tokensRepository.GetByContactAddress("1234");
            //Console.WriteLine(token.SocialLinks.WebsiteLink);

            //foreach (var t in token.Holders)
            //{
            //    Console.WriteLine(t.Soc);
            //}

            //await marketRepository.Create(new List<MarketData>() { new MarketData() { ContactAddress = "69", CurrentSupply = 100, MarketCap = 1100000, Price = .001, Timestamp = DateTime.Now }, new MarketData() { ContactAddress = "73", CurrentSupply = 100, MarketCap = 1100000, Price = .001, Timestamp = DateTime.Now } });

            await marketRepository.Delete(new List<string>() { "69", "73"} );

            List<MarketData> marketData = await marketRepository.GetByContactAddress("69");
            List<MarketData> marketsData = await marketRepository.GetByContactAddress("73");

            foreach (var m in marketData)
            {
                Console.WriteLine(m.MarketCap);
                
            }
            foreach(var m in marketsData)
            {
                Console.WriteLine(m.ContactAddress);
            }
        }
    }
}
