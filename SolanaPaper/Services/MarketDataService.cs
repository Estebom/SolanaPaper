using SolanaPaper.Data.Models;
using SolanaPaper.Data.Repositories;

namespace SolanaPaper.Services
{
    public class MarketDataService
    {
        MarketDataRepository _marketDataRepository;

        public MarketDataService(MarketDataRepository marketDataRepository)
        {
            _marketDataRepository = marketDataRepository;
        }

        public async Task<List<MarketData>> GetByContactAddress(string contactAddress)
        {
            return await _marketDataRepository.GetByContactAddress(contactAddress);
        }

        public async Task Create(MarketData marketData)
        {
            await _marketDataRepository.Create(marketData);
            return;
        }

        public async Task Create(List<MarketData> marketData)
        {
            await _marketDataRepository.Create(marketData);
            return;
        }

        public async Task Delete(string contactAddress)
        {
            await _marketDataRepository.Delete(contactAddress);
            return;
        }

        public async Task Delete(List<string> contactAddresses)
        {
            await _marketDataRepository.Delete(contactAddresses);
            return;
        }
    }
}
