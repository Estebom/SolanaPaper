using SolanaPaper.Data.Models;
using SolanaPaper.Data.Repositories;

namespace SolanaPaper.Services
{
    public class TokensService
    {
        TokenRepository _tokenRepository;

        public TokensService(TokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<List<Tokens>> Get()
        {
            return await _tokenRepository.Get();
        }

        public async Task<Tokens> GetByContactAddress(string contactAddress)
        {
            return await _tokenRepository.GetByContactAddress(contactAddress);
        }

        public async Task<List<Tokens>> GetBySymbol(string symbol)
        {
            return await _tokenRepository.GetBySymbol(symbol);
        }
        public async Task Create(Tokens token)
        {
            await _tokenRepository.Create(token);
            return;
        }

        public async Task UpdateMintAuthority(string contactAddress, string mintAuthority)
        {
            await _tokenRepository.UpdateMintAuthority(contactAddress, mintAuthority);
            return;
        }

        public async Task AddHolder(string contactAddress, Holder holder)
        {
            await _tokenRepository.AddHolder(contactAddress, holder);
            return;
        }

        public async Task AddHolders(string contactAddress, List<Holder> holders)
        {
            await _tokenRepository.AddHolder(contactAddress, holders);
            return;
        }

        public async Task RemoveHolder(string contactAddress, Holder holder)
        {
            await _tokenRepository.RemoveHolder(contactAddress, holder);
            return;
        }

        public async Task RemoveHolders(string contactAddress, List<Holder> holders)
        {
            await _tokenRepository.RemoveHolder(contactAddress, holders);
            return;
        }

        public async Task UpdateSocials(string contactAddress, Socials socials)
        {
            await _tokenRepository.UpdateSocials(contactAddress, socials);
            return;
        }

        public async Task Delete(string contactAddress)
        {
            await _tokenRepository.Delete(contactAddress);
            return;
        }
    }
}
