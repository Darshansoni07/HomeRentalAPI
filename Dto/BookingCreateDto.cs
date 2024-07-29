using System.ComponentModel.DataAnnotations;

namespace HomeRent.Dto
{
    public class BookingCreateDto
    {
        public string Amount { get; set; }
        public string contact { get; set; } = null!;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int activate { get; set; }
        public int PropertiesID { get; set; } 
        public int userId { get; set; }
    }
}
