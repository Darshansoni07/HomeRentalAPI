using System.ComponentModel.DataAnnotations;

namespace HomeRent.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        [Required]
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }
        //public string contact {  get; set; }
        //public string other_details { get; set; }
        public byte agent {  get; set; }
        public string profile_Img { get; set; }
        public bool RequestAgentAccess { get; set; }
        public bool IsEmailVerified { get; set; }

        //public virtual ICollection<HomeBook> Booking { get; set; }
    }
}
