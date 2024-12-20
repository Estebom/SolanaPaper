namespace SolanaPaper.Data.Models
{
    namespace SolanaPaper.Data.Models
    {
        public class LiveTokenPrice
        {
            public DEXTradeData Solana { get; set; }
        }

        public class DEXTradeData
        {
            public List<DEXTradeByTokens> DEXTradeByTokens { get; set; }
        }

        public class DEXTradeByTokens
        {
            public Block Block { get; set; }
            public Trade Trade { get; set; }
            public Transaction Transaction { get; set; }
        }

        public class Block
        {
            public DateTime Time { get; set; }
        }

        public class Trade
        {
            public Currency Currency { get; set; }
            public Dex Dex { get; set; }
            public double Price { get; set; }
            public double PriceInUSD { get; set; }
            public Side Side { get; set; }
        }

        public class Currency
        {
            public string MintAddress { get; set; }
            public string Name { get; set; }
            public string Symbol { get; set; }
        }

        public class Dex
        {
            public string ProgramAddress { get; set; }
            public string ProtocolFamily { get; set; }
            public string ProtocolName { get; set; }
        }

        public class Side
        {
            public Currency Currency { get; set; }
        }

        public class Transaction
        {
            public string Signature { get; set; }
        }
    }
}
