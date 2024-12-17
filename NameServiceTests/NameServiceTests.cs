using System;
using System.Threading.Tasks;
using Solnet.Rpc;
using Solnet.Programs.Clients;
using Solnet.Programs.Models.NameService;
using Xunit;
using Solnet.Programs;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Http.Websocket;
using RestSharp;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using SolanaPaper.Data.Models;
using Newtonsoft.Json;
using System.Text;
using SolanaPaper.Services.Solana;


namespace NameServiceTests
{
    public class NameServiceTests
    {
        private readonly IRpcClient rpcClient;
        private readonly NameServiceClient nameServiceClient;

        public NameServiceTests()
        {
            // Set up the RPC client to connect to Solana MainNet
            rpcClient = ClientFactory.GetClient(Cluster.MainNet);

            // Initialize the NameServiceClient with the RPC client
            nameServiceClient = new NameServiceClient(rpcClient);
        }

        [Fact]
        public async Task Test_GetTokenInfoFromMintAsync()
        {

            //var client = new HttpClient();
            //client.BaseAddress = new Uri("https://api.geckoterminal.com/api/v2");

            //client.DefaultRequestHeaders.Add("Accept", "application/json;version=20230302");

            //HttpResponseMessage response = await client.GetAsync("https://api.geckoterminal.com/api/v2/networks/solana/tokens/BoUPrmUXJjG28w2jwy2fC7oDAuSnfsSNSwKo6P7Spump\r\n");

            //if (response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine("Success");
            //    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            //}
            //else
            //{
            //    Console.WriteLine("Failed");
            //}

            //var options = new RestClientOptions("https://streaming.bitquery.io")
            //{
            //    MaxTimeout = -1
            //};
            //var client = new RestClient(options);

            //var request = new RestRequest("/eap", Method.Post);
            //request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Authorization", "Bearer ory_at_ky-xwa50B_7OvFOwfauOW-cZvQeFdaCgUjhI9Bf1_xI.2Zyl29Qk3I2klchaFIMisUMparWUZ4c1PkvxIWW2hPk\t");
            //string dynamic = "4vcmc5fiawMcmFGJmGCDAza9cEZMuL3xBNAUoi97pump";
            //var body = @"{""query"":""{\n  Solana {\n    DEXTradeByTokens(\n      limit: {count: 10}\n      orderBy: {descendingByField: \""Block_Timefield\""}\n      where: {Trade: {Currency: {MintAddress: {is: \""{MintAddress}\""}}, Dex: {ProgramAddress: {is: \""6EF8rrecthR5Dkzon8Nwu78hRvfCKubJ14M5uBEwF6P\""}}, PriceAsymmetry: {lt: 0.1}}}\n    ) {\n      Block {\n        Timefield: Time(interval: {in: minutes, count: 1})\n      }\n      volume: sum(of: Trade_Amount)\n      Trade {\n        high: Price(maximum: Trade_Price)\n        low: Price(minimum: Trade_Price)\n        open: Price(minimum: Block_Slot)\n        close: Price(maximum: Block_Slot)\n      }\n      count\n    }\n  }\n}\n"",""variables"":""{}""}";
            //var sb = new StringBuilder(body);
            //sb.Replace("{MintAddress}", dynamic);
            //body = sb.ToString();

            //request.AddStringBody(body, DataFormat.Json);

            //RestResponse response = await client.ExecuteAsync(request);

            //if (response.Content == null)
            //{
            //    Console.WriteLine("Response content is null");
            //    return;
            //}

            //Console.WriteLine(response.Content);

            //OHLCVData token = JsonConvert.DeserializeObject<OHLCVData>(response.Content);

            //Console.WriteLine(token);

            //foreach (DEXTradeByTokens trade in token.Data.Solana.DEXTradeByTokens)
            //{
            //    Console.WriteLine(trade.Volume);
            //}
            //Console.WriteLine(token.Solana.DEXTradeByTokens.Count);

            BitQueryService bitQueryService = new BitQueryService();

            OHLCVData token = await bitQueryService.GetOHLCV("4vcmc5fiawMcmFGJmGCDAza9cEZMuL3xBNAUoi97pump", "6EF8rrecthR5Dkzon8Nwu78hRvfCKubJ14M5uBEwF6P", "minutes", "5", "10");
            Console.WriteLine(token);

            foreach (DEXTradeByTokens trade in token.Data.Solana.DEXTradeByTokens)
            {
                Console.WriteLine(trade.Volume);
            }

        }
    }

        //[Fact]
        //public async Task Test_GetNameOwner()
        //{
        //    // Define a sample domain (example: "example.sol")
        //    string domainName = "example";

        //    // Hash the name and derive the PDA
        //    var hashedName = NameServiceProgram.ComputeHashedName(domainName);
        //    var pda = NameServiceProgram.DeriveNameAccountKey(hashedName, null, NameServiceProgram.TokenTLD);

        //    Console.WriteLine($"Hashed Name: {hashedName}");
        //    Console.WriteLine($"Derived PDA: {pda}");

        //    // Call the RPC client to get account info
        //    var accountInfo = await rpcClient.GetAccountInfoAsync(pda);

        //    // Assert and log results
        //    Assert.NotNull(accountInfo);
        //    Console.WriteLine($"WasSuccessful: {accountInfo.WasSuccessful}");

        //    if (accountInfo.WasSuccessful && accountInfo.Result.Value != null)
        //    {
        //        Console.WriteLine($"Account Owner: {accountInfo.Result.Value.Owner}");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Name owner not found.");
        //        Console.WriteLine($"Raw Response: {accountInfo.RawRpcResponse}");
        //    }
        //}
    }


