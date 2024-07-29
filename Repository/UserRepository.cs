using HomeRent.Dbcontext;
using HomeRent.Dto;
using HomeRent.IRepository;
using HomeRent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HomeRent.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext dbcontext;
        private readonly IConfiguration configuration;
        public UserRepository(AppDbContext dbContext, IConfiguration configuration)
        {
            dbcontext = dbContext;
            this.configuration = configuration;
        }
        public async Task<Users> Authenticate(string username, string passwordText)
        {
            
            var user = await dbcontext.HReUsers.FirstOrDefaultAsync(s => s.UserName == username);
            if (user == null || user.PasswordKey==null )
                return null;
            if (!MatchPasswordHash(passwordText, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using(var hmac = new HMACSHA512(passwordKey)) 
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for(int i=0; i<passwordHash.Length; i++) 
                {
                    if (passwordHash[i] != password[i]) return false;

                }

                return true;
            }
        }

        public void Register(string username, string password, string email)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
            Users users = new Users();
            users.UserName = username;
            users.Password = passwordHash;
            users.PasswordKey = passwordKey;
            users.Email = email;
            users.profile_Img = @"assets/images/avatar/01.jpg";
            dbcontext.HReUsers.Add(users);
            
            //Genrate Token For sending User email okay
            var Token = CreateJWT(username);

            string subject = "Account Activation";
            string body = "Your account has been successfully registered. Click the link below to activate your account: ";
            bool emailSent = SendVerifyEmail(email, subject, body,Token);
            if (emailSent)
            {
                Console.WriteLine("Token is send successfully");
            }
        }

        //send you to email to user
        public bool SendVerifyEmail(string recipientEmail, string subject, string body, string token  )
        {
            try
            {
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "mailsenderboxa@gmail.com"; // Your sender email address
                string smtpPassword = "iwfa oerp mjvi gojn"; // Your sender email password or app-specific password
                string baseurl = $"https://localhost:7193/Account/verify?token={token}";
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    body += $"<a href='{baseurl}'>Click here to verify your account</a>";
                    MailMessage mailMessage = new MailMessage(smtpUsername, recipientEmail, subject, body);
                    mailMessage.IsBodyHtml = true;
                    smtpClient.Send(mailMessage);
                }
                return true;
            }
            catch (Exception ex)
            { 
                // Log or handle the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> VerifyEmailUpdate(string username)
        {
            // Find user by username in the database
            var user = await dbcontext.HReUsers.FirstOrDefaultAsync(u => u.UserName == username);

            if (user != null)
            {
                // Update the user's email verification status or perform verification logic
                user.IsEmailVerified = true; // Assuming there's a property like EmailVerified in your user entity
                //await dbcontext.HReUsers.SaveChangesAsync();
                await dbcontext.SaveChangesAsync();
                return true; // Successfully updated email verification
            }

            return false; // User not found or verification failed
        }

        public string CreateJWT(string username)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            //JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(secretKey));
            /* var key = new SymmetricSecurityKey(Encoding.UTF8
                 .GetBytes("thisismycustomSecretkeyforauthentication"));*/

            var claims = new Claim[] {

                new Claim(ClaimTypes.Name,username)

            };

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var takenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(1000),
                SigningCredentials = signingCredentials
            };

            //Token Handler this is reponsible to gen JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(takenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> UserAlreadyExists(string userName)
        {
            return await dbcontext.HReUsers.AnyAsync(s => s.UserName == userName);
        }


        //for Send Request to Admin
        public async Task<bool> RequestAgentAccess(int userId)
        {
            var user = await dbcontext.HReUsers.FindAsync(userId);
            if (user != null)
            {
                user.RequestAgentAccess = true;
                dbcontext.HReUsers.Update(user);
                await dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        
        //Admin Approve agent request of guest
        //send email to user 
        public async Task<bool> ApproveAgentAccess(int userId)
        {
            var user = await dbcontext.HReUsers.FindAsync(userId);
            var email = user.Email;

            string recipientEmail = email; // Use the user's email from the database
            string subject = "Agent Access Approved";
            string body = "Dear User,\n\nYour agent access has been approved successfully.\n\nRegards,\nHomeRent Application";

            bool emailSent = SendEmail(recipientEmail, subject, body);
            if (emailSent)
            {
                if (user != null && user.RequestAgentAccess)
                {
                    user.agent = 1; //Assuming '1' indicates agent access granted
                    user.RequestAgentAccess = false; // Reset the request flag
                    dbcontext.HReUsers.Update(user);
                    await dbcontext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        //send you to email to user
        private bool SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "mailsenderboxa@gmail.com"; // Your sender email address
                string smtpPassword = "iwfa oerp mjvi gojn"; // Your sender email password or app-specific password
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    MailMessage mailMessage = new MailMessage(smtpUsername, recipientEmail, subject, body);
                    smtpClient.Send(mailMessage);
                }
                return true;

            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }
   

        //Getting Agent Details 
        public async Task<List<UserAgentReqDto>> GetUsersWithAgentRequest()
        {
            return await dbcontext.HReUsers
               .Where(u => u.RequestAgentAccess)
               .Select(u => new UserAgentReqDto
               {
                   Id = u.Id,
                   UserName = u.UserName,
                   Email = u.Email,
                   agent = u.agent,
                   RequestAgentAccess = u.RequestAgentAccess,
                   profile_Img = u.profile_Img
               })
               .ToListAsync();
            //return await dbcontext.HReUsers.Where(u => u.RequestAgentAccess).ToListAsync();
        }

        //Getting All Guest Details 
        public async Task<List<UserAgentReqDto>> GetAllGuestUserDetails()
        {
            return await dbcontext.HReUsers
                .Where(u => u.agent == 0 && u.UserName != "Admin")
                .Select(u => new UserAgentReqDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    agent = u.agent,
                    RequestAgentAccess = u.RequestAgentAccess,
                    profile_Img = u.profile_Img
                })
                .ToListAsync();

            //throw new NotImplementedException();
        }

/*        public async Task<Properties> GetPropertyById(int id)
        {
            return await _context.HReProperties.FindAsync(id);
        }*/

        //Getting Guest Details By ID 
        public async Task<Users> GetDetialById(int id)
        {
            return await dbcontext.HReUsers.FindAsync(id);

            
        }

        //Getting Agent List
        public async Task<List<UserAgentReqDto>> GetAllAgentUserDetails()
        {
            return await dbcontext.HReUsers
                .Where(u => u.agent == 1 && u.UserName != "Admin")
                .Select(u => new UserAgentReqDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    agent = u.agent,
                    RequestAgentAccess = u.RequestAgentAccess,
                    profile_Img = u.profile_Img
                })
                .ToListAsync();
        }

        public async Task<Users> UpdateUser(int id, Users users)
        {
            if (id != users.Id)
            {
                throw new InvalidOperationException("ID mismatch");
            }
            var user = await dbcontext.HReUsers.FindAsync(id);
            if (user.Id != null) 
            {
                user.profile_Img = users.profile_Img;
                user.Email = users.Email;
                dbcontext.Entry(user).State = EntityState.Modified;
                await dbcontext.SaveChangesAsync();

                return user;
            }
            return users;
            
        }
    }
}