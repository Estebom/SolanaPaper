using Solnet.Programs.Clients;
using Solnet.Rpc;
using Xunit;

public class Program
{

   
  

    [Fact]
    public async Task testNameService()
    {
        var client = new NameServiceClient(ClientFactory.GetClient(Cluster.MainNet));

        var result = await client.GetTokenInfoFromMintAsync("B5WTLaRwaUQpKk7ir1wniNB6m5o8GgMrimhKMYan2R6B");

        if (result.WasSuccessful)
        {
            Console.WriteLine($"Token Name: {result.ParsedResult.Value}");
        }
        else
        {
            Console.WriteLine("Token data not found.");
        }
    }
}