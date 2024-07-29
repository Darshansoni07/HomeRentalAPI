using System.ComponentModel.DataAnnotations;

namespace HomeRent.Dto
{
    public class loginReqDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        
        [Required]
        public string Password { get; set; } = null!;
    }
}
