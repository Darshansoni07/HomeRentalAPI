using AutoMapper;
using HomeRent.Dto;
using HomeRent.IRepository;
using HomeRent.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BookingController(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _uow.BookingRepository.GetBookingsAsync();
            var bookingDto = _mapper.Map<IEnumerable<BookingCreateDto>>(bookings);
            return Ok(bookingDto);
        }

        [HttpGet("getallBookedByUser/{userId}")]
        public async Task<IActionResult> GetBookByUser(int userId)
        {
            var bookings = await _uow.BookingRepository.GetBookingsByMyProUserAsync(userId);
            //var bookingDto = _mapper.Map<IEnumerable<BookingCreateDto>>(bookings);
            return Ok(bookings);
        }

        [HttpGet("getUserOwnBooking/{userId}")]
        public async Task<IActionResult> GetUserBookings(int userId)
        {
            var bookings = await _uow.BookingRepository.GetMyBookingsAsync(userId);
            //var bookingDto = _mapper.Map<IEnumerable<BookingCreateDto>>(bookings);
            return Ok(bookings);
        }


        [HttpGet("get-property-detial-book/{userId}")]
        public async Task<IActionResult> GetPropertyBookUser(int userId)
        {
            var bookings = await _uow.BookingRepository.GetMyPropertyBookAsync(userId);
            //var bookingDto = _mapper.Map<IEnumerable<BookingCreateDto>>(bookings);
            return Ok(bookings);
        }


        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _uow.BookingRepository.GetBookingByIdAsync(id);
            var bookingDto = _mapper.Map<IEnumerable<BookingCreateDto>>(booking);
            if (booking == null)
            {
                return Ok("No Data Available");
            }
            return Ok(bookingDto);
        }

        [HttpPost("create")] 
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDto bookingCreateDto)
        {           
            try
            {
                var booking = _mapper.Map<HomeBook>(bookingCreateDto);
                await _uow.BookingRepository.CreateBookingAsync(booking);
                await _uow.SaveAsync();

                return Ok("Booking created successfully");
            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message); // Return a custom error message
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] HomeBook booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBooking = await _uow.BookingRepository.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            existingBooking.Amount = booking.Amount; // Update other properties accordingly
            await _uow.BookingRepository.UpdateBookingAsync(existingBooking);

            return Ok("Booking updated successfully");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var existingBooking = await _uow.BookingRepository.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            await _uow.BookingRepository.DeleteBookingAsync(existingBooking);

            return Ok("Booking deleted successfully");
        }

    }
}
