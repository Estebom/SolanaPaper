using RestSharp;
using SolanaPaper.Data.Models;
using SolanaPaper.Data.Models.ServiceSettings;
using Newtonsoft.Json;
using System.Text;
using TradingView.Blazor.Models;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;
using SolanaPaper.Data.Models.SolanaPaper.Data.Models;
using GraphQL.Client.Http;
using System.Reactive.Disposables;
using WebSocketSharp;
using System.Net.WebSockets;
using Newtonsoft.Json.Linq;
using Solnet.Programs.Abstract;
using System.Collections.ObjectModel;



namespace SolanaPaper.Services.Solana
{
    public class BitQueryService
    {

        private readonly RestClient bitClient;

        public BitQueryService() 
        {
            bitClient = new RestClient(new RestClientOptions("https://streaming.bitquery.io"));
        }

        public async Task StreamTokenPrice(ObservableCollection<Ohlcv> ohlcvs, string contactAddress, string programId)
        {
            try
            {
                string query = @"
                          subscription MyQuery {
                              Solana {
                                DEXTradeByTokens(
                                  orderBy: {ascending: Block_Time}
                                  where: {Trade: {Dex: {ProgramAddress: {is: ""{ProgramAddress}""}}, Currency: {MintAddress: {is: ""{MintAddress}""}}}, Transaction: {Result: {Success: true}}}
                                ) {
                                  Block {
                                    Time
                                  }
                                  Trade {
                                    Currency {
                                      MintAddress
                                      Name
                                      Symbol
                                    }
                                    Dex {
                                      ProtocolName
                                      ProtocolFamily
                                      ProgramAddress
                                    }
                                    Price
                                    PriceInUSD
                                  }
                                  Transaction {
                                    Signature
                                  }
                                  volume: sum(of: Trade_Amount)
                                }
                              }
                            }";
                StringBuilder sb = new StringBuilder(query);
                sb.Replace("{MintAddress}", contactAddress);
                sb.Replace("{ProgramAddress}", programId);
                query = sb.ToString();

                var handler = new WebSocketHandler(ohlcvs);
                var cts = new CancellationTokenSource();

                var webSocketTask = handler.HandleWebSocket(query);
                var emitTask = handler.StartEmitLoop();

                var monitorTask = Task.Run(async() =>
                {
                    while (ohlcvs.Count < 300)
                    {
                        await Task.Delay(500);
                    }
                    cts.Cancel();
                });
                await Task.WhenAny(monitorTask);
                handler.StopWebSocket();
                Console.WriteLine("Stopped WebSocket after reaching 5 entries.");

            }
            catch (System.Net.WebSockets.WebSocketException wsEx)
            {
                Console.WriteLine($"WebSocket Error: {wsEx.Message}");
                if (wsEx.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {wsEx.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }
        public async Task<OhlcvVM> GetOHLCV(string contactAddress, string programId= "6EF8rrecthR5Dkzon8Nwu78hRvfCKubJ14M5uBEwF6P", string unitOfTime="seconds", string counter="1")
        {
            try
            {
                var request = new RestRequest("/eap", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {BitQuerySettings.AccessToken}\t");
                var body = @"{""query"":""{\n  Solana {\n    DEXTradeByTokens(\n      orderBy: {ascendingByField: \""Block_Timefield\""}\n      where: {Trade: {Currency: {MintAddress: {is: \""{MintAddress}\""}}, Dex: {ProgramAddress: {is: \""{ProgramAddress}\""}}, PriceAsymmetry: {lt: 0.1}}}\n    ) {\n      Block {\n        Timefield: Time(interval: {in: {unitOfTime}, count: {counter}})\n      }\n      volume: sum(of: Trade_Amount)\n      Trade {\n        high: Price(maximum: Trade_Price)\n        low: Price(minimum: Trade_Price)\n        open: Price(minimum: Block_Slot)\n        close: Price(maximum: Block_Slot)\n      }\n      count\n    }\n  }\n}\n"",""variables"":""{}""}";
                StringBuilder sb = new StringBuilder(body);
                sb.Replace("{MintAddress}", contactAddress);
                sb.Replace("{ProgramAddress}", programId);
                sb.Replace("{unitOfTime}", unitOfTime);
                sb.Replace("{counter}", counter);
                body = sb.ToString();

                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = await bitClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    return JsonConvert.DeserializeObject<OhlcvVM>(response.Content);
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

        public async Task<OhlcvVM> GetOHLCV(string contactAddress, DateTime after, string programId = "6EF8rrecthR5Dkzon8Nwu78hRvfCKubJ14M5uBEwF6P", string unitOfTime = "seconds", string counter = "1")
        {
            try
            {
                var request = new RestRequest("/eap", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {BitQuerySettings.AccessToken}\t");
                var body = @"{""query"":""{\n  Solana {\n    DEXTradeByTokens(\n      orderBy: {ascendingByField: \""Block_Timefield\""}\n      where: {Trade: {Currency: {MintAddress: {is: \""{MintAddress}\""}}, Dex: {ProgramAddress: {is: \""{ProgramAddress}\""}}, PriceAsymmetry: {lt: 0.1}}, Block: {Time: {after: \""{Time}\""}}}\n    ) {\n      Block {\n        Timefield: Time(interval: {in: {unitOfTime}, count: {counter}})\n      }\n      volume: sum(of: Trade_Amount)\n      Trade {\n        high: Price(maximum: Trade_Price)\n        low: Price(minimum: Trade_Price)\n        open: Price(minimum: Block_Slot)\n        close: Price(maximum: Block_Slot)\n      }\n      count\n    }\n  }\n}\n"",""variables"":""{}""}"; StringBuilder sb = new StringBuilder(body);
                sb.Replace("{MintAddress}", contactAddress);
                sb.Replace("{ProgramAddress}", programId);
                sb.Replace("{Time}", after.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                sb.Replace("{unitOfTime}", unitOfTime);
                sb.Replace("{counter}", counter);
                body = sb.ToString();

                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = await bitClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    return JsonConvert.DeserializeObject<OhlcvVM>(response.Content);
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
