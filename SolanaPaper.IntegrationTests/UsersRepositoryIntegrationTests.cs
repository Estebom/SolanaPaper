using Xunit;
using MongoDB.Driver;
using SolanaPaper.Data.Models;
using SolanaPaper.Data.Repositories;
using Microsoft.Extensions.Options;
using Xunit.Sdk;
using SolanaPaper.Services.Solana;
using Solnet.Rpc;
using Solnet.Programs.Models.NameService;
using Solnet.Programs.Clients;
using Solnet.Rpc.Types;
using System.Text;
using Solnet.Rpc.Models;
using Solnet.Programs;
using Solnet.Programs.Models;
using Solnet.Raydium.Types;
using Solnet.Rpc;
using Solnet.Rpc.Builders;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Models;
using Solnet.Rpc.Types;
using Solnet.Wallet;
using System.Diagnostics;
using System.Security.Cryptography;
using Solnet.Raydium.Client;
using Solnet.Raydium;

namespace SolanaPaper.IntegrationTests
{
    public class UsersRepositoryIntegrationTests
    {

        //TokenDataFetcherService tokenService;
        //IRpcClient rpcClient;
        //NameServiceClient nameServiceClient;

        //public UsersRepositoryIntegrationTests()
        //{
        //    rpcClient = ClientFactory.GetClient(Cluster.MainNet);
        //    nameServiceClient = new NameServiceClient(rpcClient);
        //}

        //[Fact]
        //public async Task Test_GetTokenInfoFromMintAsync()
        //{
        //    //var tokenService = new TokenDataFetcherService();



        //    var result = await nameServiceClient.GetTokenInfoFromMintAsync("B5WTLaRwaUQpKk7ir1wniNB6m5o8GgMrimhKMYan2R6B");
        //    if (result.WasSuccessful)
        //    {
        //        Console.WriteLine($"Token Name: {result.ParsedResult.Value.Ticker}");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Token data not found.");
        //        Console.WriteLine(result.ParsedResult.AccountAddress);
        //    }
        //}

        [Fact]
        public async Task GetRaydiumPoolAddressByMint()
        {
            var rpcClient = ClientFactory.GetClient(Cluster.MainNet);

            // Define constants
            var RAYDIUM_AMM_PROGRAM_ID = new PublicKey("675kPX9MHTjS2zt1qfr1NYHuzeLXfQM9H24wFSUt1Mp8");
            var solMint = new PublicKey("So11111111111111111111111111111111111111112");
            var tokenMint = new PublicKey("B5WTLaRwaUQpKk7ir1wniNB6m5o8GgMrimhKMYan2R6B");

            // Define the data size for the liquidity pool
            const int LIQUIDITY_STATE_LAYOUT_V4_SPAN = 624;

            // Fetch program accounts with filters
            var result = await rpcClient.GetProgramAccountsAsync(
                RAYDIUM_AMM_PROGRAM_ID,
                Commitment.Finalized,
                LIQUIDITY_STATE_LAYOUT_V4_SPAN,
                new List<MemCmp>
                {
                new MemCmp { Offset = 48, Bytes = Convert.ToBase64String(solMint.KeyBytes)},  // BaseMint offset
                new MemCmp { Offset = 80, Bytes = Convert.ToBase64String(tokenMint.KeyBytes) } // QuoteMint offset
                }
            );

            if (result.WasSuccessful && result.Result.Any())
            {
                var account = result.Result.First();
                var marketData = LiquidityStateV4.Deserialize(Convert.FromBase64String(account.Account.Data[0]));

                Console.WriteLine($"Base Vault: {marketData.BaseVault}");
                Console.WriteLine($"Quote Vault: {marketData.QuoteVault}");
            }
            else
            {
                Console.WriteLine("No liquidity pool found for the specified token pair.");
            }
        }
        }
    public class LiquidityStateV4
    {
        public PublicKey BaseMint { get; set; }
        public PublicKey QuoteMint { get; set; }
        public PublicKey BaseVault { get; set; }
        public PublicKey QuoteVault { get; set; }

        public static LiquidityStateV4 Deserialize(ReadOnlySpan<byte> data)
        {
            return new LiquidityStateV4
            {
                BaseMint = new PublicKey(data.Slice(48, 32).ToArray()),  // Adjust offsets based on layout
                QuoteMint = new PublicKey(data.Slice(80, 32).ToArray()), // Adjust offsets based on layout
                BaseVault = new PublicKey(data.Slice(128, 32).ToArray()),
                QuoteVault = new PublicKey(data.Slice(160, 32).ToArray())
            };
        }
    }
}
