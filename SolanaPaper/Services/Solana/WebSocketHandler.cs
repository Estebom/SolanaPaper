namespace SolanaPaper.Services.Solana
{
    using Newtonsoft.Json.Linq;
    using SolanaPaper.Data.Models.ServiceSettings;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.WebSockets;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using TradingView.Blazor.Models;

    public class WebSocketHandler
    {
        private Dictionary<DateTime, Ohlcv> ohlcvData = new Dictionary<DateTime, Ohlcv>();
        public ObservableCollection<Ohlcv> _ohlcvs;
        private ClientWebSocket client;
        private CancellationTokenSource cts;

        public WebSocketHandler(ObservableCollection<Ohlcv> ohlcvs)
        {
            _ohlcvs = ohlcvs;
            cts = new CancellationTokenSource();
        }

        public async Task HandleWebSocket(string query)
        {
            try
            {
                client = new ClientWebSocket();

                client.Options.SetRequestHeader("Authorization", $"Bearer {BitQuerySettings.AccessToken}");
                    client.Options.AddSubProtocol("graphql-ws");

                    //client.Options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    var uri = new Uri($"wss://streaming.bitquery.io/eap");
                    Console.WriteLine("Connecting to WebSocket...");
                    await client.ConnectAsync(uri, cts.Token);
                    Console.WriteLine("Connected!");

                    var connectionInitMessage = "{\"type\":\"connection_init\",\"payload\":{}}";
                    await client.SendAsync(
                        new ArraySegment<byte>(Encoding.UTF8.GetBytes(connectionInitMessage)),
                        WebSocketMessageType.Text,
                        true,
                        cts.Token
                    );
                    Console.WriteLine("Sent connection_init.");

                    var queryMessage = new
                    {
                        id = "1",
                        type = "start",
                        payload = new
                        {
                            query = query,
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
                    while (client.State == WebSocketState.Open && !cts.Token.IsCancellationRequested)
                    {
                        var result = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cts.Token);
                        var message = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                        Console.WriteLine($"[WebSocket Message] {message}");

                        if (!string.IsNullOrWhiteSpace(message))
                        {
                            OnWebSocketMessage(message);
                        }
                    }
            }
            catch (WebSocketException wsEx)
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

        public void OnWebSocketMessage(string message)
        {
            try
            {
                // Parse the WebSocket message and aggregate data
                JObject parsedJson = JObject.Parse(message);

                var trades = parsedJson["payload"]?["data"]?["Solana"]?["DEXTradeByTokens"];

                if (trades != null)
                {
                    foreach (var trade in trades)
                    {
                        decimal priceInUSD = trade["Trade"]["PriceInUSD"].Value<decimal>();
                        decimal price = trade["Trade"]["Price"].Value<decimal>();

                        var tradeTime = trade["Block"]["Time"].Value<DateTime>();
                        var tradePrice = 1_000_000_000m * priceInUSD;
                        var tradeVolume = priceInUSD * trade["volume"].Value<decimal>();
                        Console.WriteLine(tradeVolume);

                        AggregateTradeData(tradeTime, tradePrice, tradeVolume);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        private void AggregateTradeData(DateTime tradeTime, decimal tradePrice, decimal tradeVolume)
        {
            var intervalStart = new DateTime(tradeTime.Year, tradeTime.Month, tradeTime.Day, tradeTime.Hour, tradeTime.Minute, tradeTime.Second, 0);

            if (!ohlcvData.ContainsKey(intervalStart))
            {
                ohlcvData[intervalStart] = new Ohlcv
                {
                    Time = intervalStart,
                    Open = tradePrice,
                    High = tradePrice,
                    Low = tradePrice,
                    Close = tradePrice,
                    Volume = tradeVolume
                };
            }
            else
            {
                var candle = ohlcvData[intervalStart];
                candle.High = Math.Max(candle.High, tradePrice);
                candle.Low = Math.Min(candle.Low, tradePrice);
                candle.Close = tradePrice;
                candle.Volume += tradeVolume;
            }
        }

        private void EmitCompletedCandles()
        {
            var now = DateTime.UtcNow;
            var keysToRemove = new List<DateTime>();

            foreach (var ohlcv in ohlcvData)
            {
                if (ohlcv.Key.AddSeconds(1) <= now)
                {
                    var candle = ohlcv.Value;
                    Console.WriteLine($"Candle: {candle.Time}, Open: {candle.Open}, High: {candle.High}, Low: {candle.Low}, Close: {candle.Close}, Volume: {candle.Volume}");
                    _ohlcvs.Add(candle);
                    Console.WriteLine(_ohlcvs.Count);
                    keysToRemove.Add(ohlcv.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                ohlcvData.Remove(key);
            }
        }

        public async Task StartEmitLoop()
        {
            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    EmitCompletedCandles();
                    await Task.Delay(1000, cts.Token);
                }
            }
        catch (TaskCanceledException)
        {
                Console.WriteLine("Emit loop cancelled.");
            }
        }

        public void StopWebSocket()
        {
            Console.WriteLine("Stopping WebSocket...");
            cts.Cancel();
            client?.Dispose();
            Console.WriteLine("WebSocket stopped.");
        }
    }
}
