﻿using RestSharp;
using SolanaPaper.Data.Models;
using SolanaPaper.Data.Models.ServiceSettings;
using Newtonsoft.Json;
using System.Text;

namespace SolanaPaper.Services.Solana
{
    public class BitQueryService
    {
        private readonly RestClient bitClient;

        public BitQueryService() 
        {
            bitClient = new RestClient(new RestClientOptions("https://streaming.bitquery.io"));
        }


        public async Task<OHLCVData> GetOHLCV(string contactAddress, string programId, string unitOfTime, string count, string counter)
        {
            try
            {
                var request = new RestRequest("/eap", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {BitQuerySettings.AccessToken}\t");
                var body = @"{""query"":""{\n  Solana {\n    DEXTradeByTokens(\n      limit: {count: {count}}\n      orderBy: {descendingByField: \""Block_Timefield\""}\n      where: {Trade: {Currency: {MintAddress: {is: \""{MintAddress}\""}}, Dex: {ProgramAddress: {is: \""{ProgramAddress}\""}}, PriceAsymmetry: {lt: 0.1}}}\n    ) {\n      Block {\n        Timefield: Time(interval: {in: {unitOfTime}, count: {counter}})\n      }\n      volume: sum(of: Trade_Amount)\n      Trade {\n        high: Price(maximum: Trade_Price)\n        low: Price(minimum: Trade_Price)\n        open: Price(minimum: Block_Slot)\n        close: Price(maximum: Block_Slot)\n      }\n      count\n    }\n  }\n}\n"",""variables"":""{}""}";

                StringBuilder sb = new StringBuilder(body);
                sb.Replace("{MintAddress}", contactAddress);
                sb.Replace("{ProgramAddress}", programId);
                sb.Replace("{unitOfTime}", unitOfTime);
                sb.Replace("{count}", count);
                sb.Replace("{counter}", counter);
                body = sb.ToString();

                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = await bitClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    return JsonConvert.DeserializeObject<OHLCVData>(response.Content);
                }
                else
                {
                    Console.WriteLine("Response content is null");
                    return null;
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
