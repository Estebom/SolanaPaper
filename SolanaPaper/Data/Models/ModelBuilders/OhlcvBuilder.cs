using System.Security.Cryptography.X509Certificates;
using TradingView.Blazor.Models;

namespace SolanaPaper.Data.Models.ModelBuilders
{
    public static class OhlcvBuilder
    {

        public static List<Ohlcv> OhlcvModelBuilder(OHLCVData ohlcv) 
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

                ohlcvModels.Add(ohlcvModel);
            }

            return ohlcvModels;
        }

        public static List<List<Ohlcv>> OhlcvModelBuilder(List<OHLCVData> ohlcvs) 
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
