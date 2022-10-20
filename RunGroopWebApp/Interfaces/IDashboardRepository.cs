using ClubGroopWebApp.Models;

namespace ClubGroopWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUserClub();

        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetUserByIdNoTraking(string id);
        bool Update(AppUser user);
        bool Save();
       // bool Delete(AppUser user);
    }
}
