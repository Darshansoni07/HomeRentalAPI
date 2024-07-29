using HomeRent.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Dbcontext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }       
        public DbSet<City> HomeReCity { get; set; }
        public DbSet<Users> HReUsers { get; set; }
        public DbSet<Properties> HReProperties { get; set; }
        public DbSet<HomeBook> HomeRBooking { get; set; }   
    }  
}
