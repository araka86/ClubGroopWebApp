using Microsoft.EntityFrameworkCore;
using ClubGroopWebApp.Models;
using ClubGroopWebApp.Data;
using ClubGroopWebApp.Interfaces;
using ClubGroopWebApp.Data.Enum;

namespace ClubGroopWebApp.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ClubRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public bool Add(Club club)
        {
            _applicationDbContext.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _applicationDbContext.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _applicationDbContext.Clubs.ToListAsync();
        }

        public async Task<List<State>> GetAllStates()
        {
            return await _applicationDbContext.States.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Clubs.Include(y => y.Address).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Club> GetByIdAsyncNoTracking(int id)
        {
            return await _applicationDbContext.Clubs.Include(y => y.Address).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _applicationDbContext.Clubs.Where(x => x.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<IEnumerable<Club>> GetClubsByState(string state)
        {
            return await _applicationDbContext.Clubs.Where(c => c.Address.State.Contains(state)).ToListAsync();
        }

        public  async Task<int> GetCountAsync()
        {
            return await _applicationDbContext.Clubs.CountAsync();
        }

        public Task<int> GetCountByCategoryAsync(ClubCategory category)
        {
           return _applicationDbContext.Clubs.CountAsync(c => c.ClubCategory == category);
        }

        public bool Save()
        {
            var saved = _applicationDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
           _applicationDbContext.Update(club);
            return Save();
        }
    }
}
