using System.ComponentModel.DataAnnotations;

namespace HomeRent.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }        
        [Required]
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public DateTime LastUpdatedOn { get; set; }
        public int LastUpdatedBy { get; set; }          

    }
}