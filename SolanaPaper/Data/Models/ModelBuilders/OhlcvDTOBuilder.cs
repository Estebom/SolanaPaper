using TradingView.Blazor.Models;

namespace SolanaPaper.Data.Models.ModelBuilders
{
    public static class OhlcvDTOBuilder
    {
        public static List<OhlcvDTO> OhlcvModelBuilder(OhlcvVM ohlcv, string contactAddress)
        {

            List<OhlcvDTO> ohlcvModels = new List<OhlcvDTO>();

            foreach (DEXTradeByTokens transaction in ohlcv.Data.Solana.DEXTradeByTokens)
            {
                OhlcvDTO ohlcvModel = new OhlcvDTO();
                ohlcvModel.ContactAddress = contactAddress;
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

        public static List<List<OhlcvDTO>> OhlcvModelBuilder(List<OhlcvVM> ohlcvs, string contactAddress)
        {
            List<List<OhlcvDTO>> ohlcvModels = new List<List<OhlcvDTO>>();
            foreach (OhlcvVM v in ohlcvs)
            {
                ohlcvModels.Add(OhlcvModelBuilder(v, contactAddress));
            }

            return ohlcvModels;
        }
    }
}
