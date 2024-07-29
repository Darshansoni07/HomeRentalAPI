using System.ComponentModel.DataAnnotations;

namespace HomeRent.Dto
{
    public class CityDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Mandatory")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage="Country is Mandatory")]
        public string Country {  get; set; } = null!;
    }
}
