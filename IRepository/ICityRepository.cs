using HomeRent.Models;

namespace HomeRent.IRepository
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        void AddCity(City city);
        void DeleteCity(int CityId);
        Task<City> FindCity(int id);

    }
}
