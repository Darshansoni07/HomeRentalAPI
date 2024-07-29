using System.ComponentModel.DataAnnotations;

namespace HomeRent.Dto
{
    public class SignupDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Email {  get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
