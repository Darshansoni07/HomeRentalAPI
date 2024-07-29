namespace HomeRent.Dto
{
    public class UserAgentReqDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte agent { get; set; }
        public bool RequestAgentAccess { get; set; }
        public string profile_Img { get; set; }
    }
}
