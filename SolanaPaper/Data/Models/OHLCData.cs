using Solnet.Rpc.Models;
using Solnet.Serum.Models;
using System;
using System.Collections.Generic;

namespace SolanaPaper.Data.Models
{ 
        public class OHLCData
        {
            public SolanaData Data { get; set; }
        }

        public class SolanaData
        {
            public SolanaTrades Solana { get; set; }
        }

        public class SolanaTrades
        {
            public List<DEXTradeByTokens> DEXTradeByTokens { get; set; }
        }

        public class DEXTradeByTokens
        {
            public Block Block { get; set; }
            public Trade Trade { get; set; }
            public int Count { get; set; } 
            public double Volume { get; set; } 
        }

        public class Block
        {
            public DateTime Timefield { get; set; }
        }

        public class Trade
        {
            public double Close { get; set; }
            public double High { get; set; }
            public double Low { get; set; }
            public double Open { get; set; }
        }
    

}
