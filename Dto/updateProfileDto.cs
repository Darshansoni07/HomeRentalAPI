using System.ComponentModel.DataAnnotations;

namespace HomeRent.Dto
{
    public class updateProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string profile_Img { get; set; }
    }
}
