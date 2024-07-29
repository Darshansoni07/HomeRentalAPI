using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeRent.Models
{
    public class HomeBook
    {
        [Key]
        public int BookingId { get; set; }
        [Required]
        public string Amount { get; set;}
        [Required]
        public string contact { get; set; } = null!;
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Email { get; set; }
        public DateTime LastUpdatedOn { get; set; }        
        public int activate { get; set; }
        public int userId { get; set; }
        public int PropertiesID { get; set; }
        [ForeignKey("PropertiesID")]
        public Properties Properties { get; set; }
        
    }
}
