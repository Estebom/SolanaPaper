using SolanaPaper.Data.Models;
using Solnet.Programs.Clients;
using Solnet.Programs.Models.NameService;
using Solnet.Rpc;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace SolanaPaper.Services.Solana
{
    public class TokenDataFetcherService
    {
        private readonly NameServiceClient nameServiceClient = null!;
        public TokenDataFetcherService()
        {
            var rpcClient = ClientFactory.GetClient("https://api.mainnet-beta.penis.com");
            var wRpcClient = ClientFactory.GetStreamingClient(Cluster.MainNet);

            nameServiceClient = new NameServiceClient(rpcClient);
        }

        public async Task<TokenData?> GetTokenByContactAddress(string contactAddress)
        {
            try
            {
                var result = await nameServiceClient.GetTokenInfoFromMintAsync(contactAddress);

                Console.WriteLine($"WasSuccessful: {result.WasSuccessful}");
                Console.WriteLine($"ParsedResult: {result.ParsedResult}");
                Console.WriteLine($"Raw Result: {result.OriginalRequest}");


                if (result.WasSuccessful)
                {
                    return result.ParsedResult.Value;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<TokenData?> GetTokenByTicker(string ticker)
        {
            try
            {
                var result = await nameServiceClient.GetMintFromTokenTickerAsync(ticker);
                if (result.WasSuccessful)
                {
                    string contactAddress = result.ParsedResult.Value;
                    return await GetTokenByContactAddress(contactAddress);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> GetTwitterByContactAddress(string contactAddress)
        {
            try
            {
                var result = await nameServiceClient.GetTwitterHandleFromAddressAsync(contactAddress);
                if (result.WasSuccessful)
                {
                    return result.ParsedResult.TwitterHandle;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}

