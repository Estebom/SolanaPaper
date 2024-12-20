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



namespace SolanaPaper.Services.Solana
{
    public class BitQueryService
    {

        private readonly RestClient bitClient;

        public BitQueryService() 
        {
            bitClient = new RestClient(new RestClientOptions("https://streaming.bitquery.io"));
        }

        public async Task StreamTokenPrice(List<Ohlcv> ohlcvs)
        {
            try
            {
                using (var client = new ClientWebSocket())
                {
                    client.Options.SetRequestHeader("Authorization", $"Bearer {BitQuerySettings.AccessToken}");
                    client.Options.AddSubProtocol("graphql-ws");

                    //client.Options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    var uri = new Uri($"wss://streaming.bitquery.io/eap");
                    Console.WriteLine("Connecting to WebSocket...");
                    await client.ConnectAsync(uri, CancellationToken.None);
                    Console.WriteLine("Connected!");

                    var connectionInitMessage = "{\"type\":\"connection_init\",\"payload\":{}}";
                    await client.SendAsync(
                        new ArraySegment<byte>(Encoding.UTF8.GetBytes(connectionInitMessage)),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );
                    Console.WriteLine("Sent connection_init.");

                    var queryMessage = new
                    {
                        id = "1",
                        type = "start",
                        payload = new
                        {
                            query = @"
                          subscription MyQuery {
                              Solana {
                                DEXTradeByTokens(
                                  orderBy: {ascending: Block_Time}
                                  where: {Trade: {Dex: {ProgramAddress: {is: ""6EF8rrecthR5Dkzon8Nwu78hRvfCKubJ14M5uBEwF6P""}}, Currency: {MintAddress: {is: ""7a1tvjPAh5j47CL9fus9hjfY5RRgyKg5xmQJLJWSpump""}}}, Transaction: {Result: {Success: true}}}
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
                            }",
                            variables = new { }
                        }
                    };

                    var queryMessageJson = Newtonsoft.Json.JsonConvert.SerializeObject(queryMessage);
                    await client.SendAsync(
                        new ArraySegment<byte>(Encoding.UTF8.GetBytes(queryMessageJson)),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );
                    Console.WriteLine("Sent query.");


                    var receiveBuffer = new byte[1100];

                    // Listen for messages
                    var receiveTask = Task.Run(async () =>
                    {
                        while (client.State == System.Net.WebSockets.WebSocketState.Open)
                        {
                            var result = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                            var message = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                            Console.WriteLine($"[WebSocket Message] {message}");

                            //JObject parsedJson = JObject.Parse(message);

                            //var trades = parsedJson["payload"]?["data"]?["Solana"]?["DEXTradeByTokens"];

                            //if (trades != null) 
                            //{
                            //    foreach (var trade in trades) 
                            //    {
                            //        //ohlcvs.Insert(0, new PricePoint
                            //        //{
                            //        //    Time = trade["Block"]["Time"].Value<DateTime>(),
                            //        //    Price = trade["Trade"]["Price"].Value<decimal>(),
                            //        //    Volume = trade["volume"].Value<decimal>()
                            //        //});
                            //    }
                            //}
                        }
                    });

                    // Send messages
                    while (true)
                    {
                        Console.Write("> ");
                        var msg = Console.ReadLine();

                        if (msg == "exit")
                        {
                            break;
                        }

                        var msgBuffer = Encoding.UTF8.GetBytes(msg);
                        await client.SendAsync(new ArraySegment<byte>(msgBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }

                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                }
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
        public async Task<OhlcvVM> GetOHLCV(string contactAddress, string programId= "6EF8rrecthR5Dkzon8Nwu78hRvfCKubJ14M5uBEwF6P", string unitOfTime="minutes", string counter="1")
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
    }
}
