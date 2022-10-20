using ClubGroopWebApp.Models;

namespace ClubGroopWebApp.Interfaces
{
    public interface IRaceRepository
    {

        Task<IEnumerable<Race>> GetAll();
        Task<Race> GetByIdAcync(int id);
        Task<Race> GetByIdAcyncNotracking(int id);
        Task<IEnumerable<Race>> GetAllRecesByCity(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();


    }
}
