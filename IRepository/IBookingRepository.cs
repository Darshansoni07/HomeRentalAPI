using HomeRent.Models;

namespace HomeRent.IRepository
{
    public interface IBookingRepository
    {
        Task<IEnumerable<HomeBook>> GetBookingsAsync();
        Task<IEnumerable<HomeBook>> GetBookingsByMyProUserAsync(int userId);
        Task<IEnumerable<HomeBook>> GetMyBookingsAsync(int userId);
        Task<IEnumerable<Properties>> GetMyPropertyBookAsync(int userId);
        Task<HomeBook> GetBookingByIdAsync(int id);
        Task CreateBookingAsync(HomeBook booking);
        Task UpdateBookingAsync(HomeBook booking);
        Task DeleteBookingAsync(HomeBook booking);
    }
}
