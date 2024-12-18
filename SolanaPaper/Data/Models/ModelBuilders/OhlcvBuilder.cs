using System.Security.Cryptography.X509Certificates;
using TradingView.Blazor.Models;

namespace SolanaPaper.Data.Models.ModelBuilders
{
    public static class OhlcvBuilder
    {

        public static List<Ohlcv> OhlcvModelBuilder(OhlcvVM ohlcv) 
        {

            List<Ohlcv> ohlcvModels = new List<Ohlcv>();

            foreach (DEXTradeByTokens transaction in ohlcv.Data.Solana.DEXTradeByTokens)
            {
                Ohlcv ohlcvModel = new Ohlcv();
                ohlcvModel.Time = transaction.Block.Timefield;
                ohlcvModel.Close = (decimal)transaction.Trade.Close;
                ohlcvModel.Open = (decimal)transaction.Trade.Open;
                ohlcvModel.High = (decimal)transaction.Trade.High;
                ohlcvModel.Low =  (decimal)transaction.Trade.Low;
                ohlcvModel.Volume = (decimal)transaction.Volume;

                ohlcvModels.Add(ohlcvModel);
            }

            return ohlcvModels;
        }

        public static List<Ohlcv> OhlcvModelBuilder(List<OhlcvDTO> ohlcvs)
        {

            List<Ohlcv> ohlcvModels = new List<Ohlcv>();

            foreach (OhlcvDTO transaction in ohlcvs)
            {
                Ohlcv ohlcvModel = new Ohlcv();
                ohlcvModel.Time = transaction.Time;
                ohlcvModel.Close = 1_000_000_000m * transaction.Close * 215;
                ohlcvModel.Open = 1_000_000_000m * transaction.Open * 215;
                ohlcvModel.High = 1_000_000_000m * transaction.High * 215;
                ohlcvModel.Low = 1_000_000_000m * transaction.Low * 215;
                ohlcvModel.Volume = transaction.Volume * transaction.Close * 215;

                ohlcvModels.Add(ohlcvModel);
            }

            return ohlcvModels;
        }

        public static List<List<Ohlcv>> OhlcvModelBuilder(List<OhlcvVM> ohlcvs) 
        {
            List<List<Ohlcv>> ohlcvModels = new List<List<Ohlcv>>();
            foreach (OhlcvVM v in ohlcvs) 
            {
                ohlcvModels.Add(OhlcvModelBuilder(v));
            }

            return ohlcvModels;
        }
    }
}
