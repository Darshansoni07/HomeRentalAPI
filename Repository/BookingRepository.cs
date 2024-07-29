using HomeRent.Dbcontext;
using HomeRent.IRepository;
using HomeRent.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _dbContext;

        public BookingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<HomeBook>> GetBookingsAsync()
        {
            return await _dbContext.HomeRBooking.ToListAsync();
        }
        public async Task<IEnumerable<HomeBook>> GetBookingsByMyProUserAsync(int userId)
        {
            string StoredProc = "exec GetOtherUsersWhoBookedMyProperty " +
               "@UserId =" + userId;
            var s1 = await _dbContext.HomeRBooking.FromSqlRaw(StoredProc).ToListAsync();
            return s1 ;             
        }

        public async Task<IEnumerable<HomeBook>> GetMyBookingsAsync(int userId)
        {
            string StoredProc = "exec UserBookingProperties " +
                "@UserId =" + userId;
            //var Propertydetials = await _dbContext.HReProperties.FromSqlRaw(StoredProc).ToListAsync();
            return await _dbContext.HomeRBooking.FromSqlRaw(StoredProc).ToListAsync();
        }

        public async Task<IEnumerable<Properties>> GetMyPropertyBookAsync(int userId)
        {
            string StoredProc = "exec UserBookingProperties " +
                "@UserId =" + userId;
             var data =await _dbContext.HReProperties.FromSqlRaw(StoredProc).ToListAsync();
            return data;
        }


        public async Task<HomeBook> GetBookingByIdAsync(int id)
        {
            return await _dbContext.HomeRBooking.FindAsync(id);
        }

        public async Task CreateBookingAsync(HomeBook booking)
        {
            var properties_data = _dbContext.HReProperties.FirstOrDefault(a =>a.property_id == booking.PropertiesID && a.UsersId != booking.userId);
            if (properties_data != null)
            {
                var mac = _dbContext.HomeRBooking.FirstOrDefault(b => b.userId == booking.userId && b.PropertiesID == booking.PropertiesID);
                
                if (mac != null)
                {
                    throw new Exception("User Already Booked");
                }
                
                _dbContext.HomeRBooking.Add(booking);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("You Can't Book this is your own home");
            }
        }

        public async Task UpdateBookingAsync(HomeBook booking)
        {
            _dbContext.HomeRBooking.Update(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(HomeBook booking)
        {
            _dbContext.HomeRBooking.Remove(booking);
            await _dbContext.SaveChangesAsync();
        }

        
    }
}
