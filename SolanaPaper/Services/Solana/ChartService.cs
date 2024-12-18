using SolanaPaper.Data.Models;
using SolanaPaper.Data.Models.ModelBuilders;
using SolanaPaper.Data.Repositories;
using TradingView.Blazor.Models;

namespace SolanaPaper.Services.Solana
{
    public class ChartService
    {
        OhlcvRepository _ohlcvRepository;
        BitQueryService _bitQueryService;
        public ChartService(OhlcvRepository ohlcvRepo, BitQueryService bitQueryService)
        {
            _ohlcvRepository = ohlcvRepo;
            _bitQueryService = bitQueryService;
        }

        public async Task GetMongoOhlcv(List<Ohlcv> entries, string mintAddress) 
        {
            try
            {
                var ohlcvDTOs = await _ohlcvRepository.GetByContactAddress(mintAddress);
                entries.AddRange(OhlcvBuilder.OhlcvModelBuilder(ohlcvDTOs));
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task GetBitQueryOhlcv(List<Ohlcv> entries, string mintAddress, string programId, string unitOfTime, string count, string counter)
        {
            try
            {
                var ohlcvVM = await _bitQueryService.GetOHLCV(mintAddress, programId, unitOfTime, count, counter);
                var newOhlcvDTOs = OhlcvDTOBuilder.OhlcvModelBuilder(ohlcvVM, mintAddress);
                await _ohlcvRepository.Create(newOhlcvDTOs);

                entries.AddRange(OhlcvBuilder.OhlcvModelBuilder(newOhlcvDTOs));
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<Ohlcv> RemoveDuplicates(List<Ohlcv> entries)
        {
            return entries
                    .GroupBy(entry => entry.Time)
                    .Select(group => group.First())
                    .OrderBy(entry => entry.Time) 
                    .ToList();
        }

        public List<PricePoint> GetPricePoints(List<Ohlcv> entries)
        {
            return entries.Select(entry => new PricePoint
            {
                Time = entry.Time,
                Price = entry.Close, // Ensure price is properly mapped
                Volume = entry.Volume
            }).ToList();
        }
    }
}
