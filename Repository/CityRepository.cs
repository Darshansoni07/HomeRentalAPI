using HomeRent.Dbcontext;
using HomeRent.IRepository;
using HomeRent.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly AppDbContext dbcontext;
        public CityRepository(AppDbContext appContext) 
        {
            dbcontext = appContext;
        }
        public void AddCity(City city)
        {
            dbcontext.HomeReCity.AddAsync(city);
        }

        public void DeleteCity(int CityId)
        {
            var city = dbcontext.HomeReCity.Find(CityId);
            dbcontext.HomeReCity.Remove(city);
            dbcontext.SaveChangesAsync();
            //throw new NotImplementedException();
        }

        public async Task<City> FindCity(int id)
        {
            return await dbcontext.HomeReCity.FindAsync(id);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await dbcontext.HomeReCity.ToListAsync();
            //throw new NotImplementedException();
        }

       
    }
}
