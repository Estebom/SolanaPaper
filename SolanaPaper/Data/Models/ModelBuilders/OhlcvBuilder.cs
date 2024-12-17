using System.Security.Cryptography.X509Certificates;
using TradingView.Blazor.Models;

namespace SolanaPaper.Data.Models.ModelBuilders
{
    public class OhlcvBuilder
    {
       
        public List<Ohlcv> OhlcvModelBuilder(OHLCVData ohlcv) 
        {
            List<Ohlcv> ohlcvModels = new List<Ohlcv>();

            foreach (DEXTradeByTokens transaction in ohlcv.Data.Solana.DEXTradeByTokens)
            {
                Ohlcv ohlcvModel = new Ohlcv();
                ohlcvModel.Time = transaction.Block.Timefield;
                ohlcvModel.Close = (decimal)transaction.Trade.Close;
                ohlcvModel.Open = (decimal)transaction.Trade.Open;
                ohlcvModel.High = (decimal)transaction.Trade.High;
                ohlcvModel.Low = (decimal)transaction.Trade.Low;
                ohlcvModel.Volume = (decimal)transaction.Volume;
            }

            return ohlcvModels;
        }

        public List<List<Ohlcv>> OhlcvModelBuilder(List<OHLCVData> ohlcvs) 
        {
            List<List<Ohlcv>> ohlcvModels = new List<List<Ohlcv>>();
            foreach (OHLCVData v in ohlcvs) 
            {
                ohlcvModels.Add(OhlcvModelBuilder(v));
            }

            return ohlcvModels;
        }
    }
}
