using ClubGroopWebApp.Data;
using ClubGroopWebApp.Interfaces;
using ClubGroopWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubGroopWebApp.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public RaceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public bool Add(Race race)
        {
            _applicationDbContext.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _applicationDbContext.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
           return await _applicationDbContext.Races.ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetAllRecesByCity(string city)
        {
            return await _applicationDbContext.Races.Where(x => x.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race> GetByIdAcync(int id)
        {
            return await _applicationDbContext.Races.Include(x => x.Address).FirstOrDefaultAsync(y => y.Id==id);
        }
        public async Task<Race> GetByIdAcyncNotracking(int id)
        {
            return await _applicationDbContext.Races.Include(x => x.Address).AsNoTracking().FirstOrDefaultAsync(y => y.Id == id);
        }

        public bool Save()
        {
            var saved = _applicationDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _applicationDbContext.Update(race);
            return Save();
        }
    }
}
