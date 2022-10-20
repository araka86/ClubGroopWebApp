using ClubGroopWebApp.Models;
using ClubGroopWebApp.Data.Enum;


namespace ClubGroopWebApp.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAll();
        Task<IEnumerable<Club>> GetClubsByState(string state);
        Task<Club> GetByIdAsync(int id);
        Task<Club> GetByIdAsyncNoTracking(int id);

        Task<List<State>> GetAllStates();
        Task<IEnumerable<Club>> GetClubByCity(string city);
        Task<int> GetCountAsync();
        Task<int> GetCountByCategoryAsync(ClubCategory category);
        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();
    }
}
