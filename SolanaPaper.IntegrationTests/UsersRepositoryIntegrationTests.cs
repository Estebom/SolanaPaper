using Xunit;
using MongoDB.Driver;
using SolanaPaper.Data.Models;
using SolanaPaper.Data.Repositories;
using Microsoft.Extensions.Options;
using Xunit.Sdk;
using SolanaPaper.Services.Solana;
using Solnet.Rpc;
using Solnet.Programs.Models.NameService;

namespace SolanaPaper.IntegrationTests
{
    public class UsersRepositoryIntegrationTests
    {

        TokenDataFetcherService tokenService;

        public UsersRepositoryIntegrationTests()
        {
            tokenService = new TokenDataFetcherService();
        }

        [Fact]
        public async Task Test_GetTokenInfoFromMintAsync()
        {
            var tokenService = new TokenDataFetcherService();

            var result = await tokenService.GetTokenByContactAddress("EPjFWdd5AufqSSqeM2qN1xzybapC8G4wEGGkZwyTDt1v");
            if (result != null)
            {
                Console.WriteLine($"Token Name: {result.Name}");
            }
            else
            {
                Console.WriteLine("Token data not found.");
            }
        }

        }
}
