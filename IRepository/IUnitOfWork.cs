namespace HomeRent.IRepository
{
    public interface IUnitOfWork
    {
        ICityRepository CityRepository { get; }
        IUserRepository UserRepository { get; }
        IPropertyRepository PropertyRepository { get; }
        IBookingRepository BookingRepository { get; }
        Task<bool> SaveAsync();
    }
}