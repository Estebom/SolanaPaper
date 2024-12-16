using SolanaPaper.Data.Models.ServiceSettings;
using Solnet.Rpc;
using Solnet.Rpc.Types;
using System.Text;

namespace SolanaPaper.Services.Solana
{
    public class PoolFinder
    {
        private readonly IRpcClient rpcClient;


        public PoolFinder(Cluster endpoint) 
        {
            rpcClient = ClientFactory.GetClient(endpoint);
        }

        public async Task<List<string>> FindPoolsByMint(string contactAddress)
        {
            try
            {
                var response = await rpcClient.GetProgramAccountsAsync(contactAddress, Commitment.Finalized);

                if (response.WasSuccessful)
                {
                    var poolKeys = new List<string>();

                    foreach (var account in response.Result)
                    {
                        var data = Convert.FromBase64String(account.Account.Data[0]);

                        string baseMint = Encoding.UTF8.GetString(data[0..32]).TrimEnd('\0');
                        string quoteMint = Encoding.UTF8.GetString(data[32..64]).TrimEnd('\0');
                        ulong baseReserve = BitConverter.ToUInt64(data[64..72]);
                        ulong quoteReserve = BitConverter.ToUInt64(data[72..80]);
                        string lpMint = Encoding.UTF8.GetString(data[80..112]).TrimEnd('\0');

                        if (baseMint == contactAddress || quoteMint == contactAddress)
                        {
                            Console.WriteLine($"Pool Found:");
                            Console.WriteLine($"  Base Mint: {baseMint}");
                            Console.WriteLine($"  Quote Mint: {quoteMint}");
                            Console.WriteLine($"  Base Reserve: {baseReserve}");
                            Console.WriteLine($"  Quote Reserve: {quoteReserve}");
                            Console.WriteLine($"  LP Mint: {lpMint}");

                            poolKeys.Add(account.PublicKey);
                        }
                    }

                    return poolKeys;
                }
                else
                {
                    Console.WriteLine("Failed to get program accounts");
                    return new List<string>();
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
