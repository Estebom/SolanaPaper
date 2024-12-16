using SolanaPaper.Data.Models;
using SolanaPaper.Data.Repositories;


namespace SolanaPaper.Services
{
    public class UsersService
    {
        private readonly UsersRepository _usersRepository;


        public UsersService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<List<Users>> Get()
        {
            return await _usersRepository.Get();
        }

        public async Task<Users> GetByUsername(string username)
        {
            return await _usersRepository.GetByUsername(username);
        }

        public async Task<Users> GetByEmail(string email)
        {
            return await _usersRepository.GetByEmail(email);
        }

        public async Task Create(Users user)
        {
            await _usersRepository.Create(user);
            return;
        }

        public async Task UpdateStats(string username, Stats stats)
        {
            await _usersRepository.UpdateStats(username, stats);
            return;
        }

        public  async Task AddToHoldings(string username, string holding)
        {
            await _usersRepository.AddToHoldings(username, holding);
            return;
        }

        public async Task AddToHoldings(string username, List<string> holdings)
        {
            await _usersRepository.AddToHoldings(username, holdings);
            return;
        }

        public async Task RemoveFromHoldings(string username, string holding)
        {
            await _usersRepository.RemoveFromHoldings(username, holding);
            return;
        }

        public async Task RemoveFromHoldings(string username, List<string> holdings)
        {
            await _usersRepository.RemoveFromHoldings(username, holdings);
            return;
        }

        public async Task Delete(string username)
        {
            await _usersRepository.Delete(username);
            return;
        }

    }
}
