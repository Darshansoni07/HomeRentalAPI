using HomeRent.Dto;
using HomeRent.Models;
using System.Net.Http.Headers;

namespace HomeRent.IRepository
{
    public interface IUserRepository
    {
        Task<Users> Authenticate(string username, string password);
        void Register(string username, string password, string email);
        Task<bool> UserAlreadyExists(string userName);
        Task<bool> RequestAgentAccess(int userId);
        Task<bool> ApproveAgentAccess(int userId);
        Task<List<UserAgentReqDto>> GetUsersWithAgentRequest();
        Task<List<UserAgentReqDto>> GetAllGuestUserDetails();
        Task<Users> GetDetialById(int id);
        Task<List<UserAgentReqDto>> GetAllAgentUserDetails();
        Task<Users> UpdateUser(int id, Users users);
        Task<bool> VerifyEmailUpdate(string username);
        public bool SendVerifyEmail(string recipientEmail, string subject, string body, string token);
        public string CreateJWT(string username);

    }
}
