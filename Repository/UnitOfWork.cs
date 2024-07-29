using HomeRent.Dbcontext;
using HomeRent.IRepository;

namespace HomeRent.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbcontext;
        private readonly IConfiguration configuration;
        public UnitOfWork(AppDbContext dbcontext, IConfiguration configuration)
        {
            this.dbcontext = dbcontext;
            this.configuration = configuration;
        }
        public ICityRepository CityRepository => new CityRepository(dbcontext);
        public IUserRepository UserRepository => new UserRepository(dbcontext,configuration);
        public IPropertyRepository PropertyRepository => new PropertyRepository(dbcontext);
        public IBookingRepository BookingRepository => new BookingRepository(dbcontext);

        public async Task<bool> SaveAsync()
        {
            return await dbcontext.SaveChangesAsync() > 0;
        }
    }
}
