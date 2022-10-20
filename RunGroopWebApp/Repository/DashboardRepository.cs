using ClubGroopWebApp.Data;
using ClubGroopWebApp.Models;
using Microsoft.EntityFrameworkCore;
using ClubGroopWebApp.Interfaces;

namespace ClubGroopWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext applicationDbContext,IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClub()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClub =  _applicationDbContext.Clubs.Where(r => r.AppUser.Id == curUser);
            return userClub.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var useRace = _applicationDbContext.Races.Where(r => r.AppUser.Id == curUser);
            return useRace.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _applicationDbContext.Users.FindAsync(id);
        }
        public async Task<AppUser> GetUserByIdNoTraking(string id)
        {
            return await _applicationDbContext.Users.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public bool Update(AppUser user)
        {
            _applicationDbContext.Users.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _applicationDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
