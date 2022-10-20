using ClubGroopWebApp.Data;
using ClubGroopWebApp.Interfaces;
using ClubGroopWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubGroopWebApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public bool Add(AppUser user)
        {
            _applicationDbContext.Add(user);
            return Save();
        }

        public bool Delete(AppUser user)
        {
            _applicationDbContext.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _applicationDbContext.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _applicationDbContext.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved = _applicationDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _applicationDbContext.Update(user);
            return Save();
        }
    }
}
