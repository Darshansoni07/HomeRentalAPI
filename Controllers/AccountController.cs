using AutoMapper;
using HomeRent.Dto;
using HomeRent.IRepository;
using HomeRent.Models;
using HomeRent.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace HomeRent.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;
        private readonly IMapper _mapper;

        public AccountController(IUnitOfWork uow, IMapper mapper, IConfiguration configuration) 
        {
            this.uow = uow;
            this.configuration = configuration;
            this._mapper = mapper;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(loginReqDto loginReqDto)
        {
            var user = await uow.UserRepository.Authenticate(loginReqDto.UserName, loginReqDto.Password);
            if(user == null)
            {
                return Ok("Not");
            }
            
            var loginRes = new LoginRespDto();
            loginRes.Username = loginReqDto.UserName;            
            loginRes.Token = CreateJWT(user);
            if (user.IsEmailVerified == false)
            {
                var Token = uow.UserRepository.CreateJWT(user.UserName);
                string subject = "Account Activation";
                string body = "Your account has been successfully registered. Click the link below to activate your account: ";
                bool emailSent = uow.UserRepository.SendVerifyEmail(user.Email, subject, body, Token);
                if (emailSent)
                {
                    Console.WriteLine("Token is send successfully");
                }
                return Ok("You have not verify email We Have sent Link");
            }
            return Ok(loginRes);  
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(SignupDto loginReqDto)
        {
            if (await uow.UserRepository.UserAlreadyExists(loginReqDto.UserName))
                //return BadRequest("User already exists, please try something else");
                return Ok("Already User exsist");
            uow.UserRepository.Register(loginReqDto.UserName,loginReqDto.Password, loginReqDto.Email);
            await uow.SaveAsync();
           
            return Ok("Success");
        }
         

        [HttpGet("verify")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false, // Modify these as needed
                    ValidateAudience = false, // Modify these as needed
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    //Accessing claims from the validated token
                    var nameClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                    var result = await uow.UserRepository.VerifyEmailUpdate(nameClaim);
                    return Ok("Email verified successfully.   You can now login.");
                }
                else
                {
                    return BadRequest("Invalid or expired token.");
                }
                //return Ok("Email verified successfully. You can now login.");
            }
            catch (Exception)
            {
                return BadRequest("Invalid or expired token."); // Token validation failed
            }
        }

        //Request send to admin
        [HttpPost("request-agent-access/{userId}")]
        public async Task<IActionResult> RequestAgentAccess(int userId)
        {
            var result = await uow.UserRepository.RequestAgentAccess(userId);
            if (result)
            {
                return Ok("Agent access requested successfully.");
            }
            return NotFound("User not found or request failed.");
        }

        //Request  approved 
        [HttpPost("approve-agent-access/{userId}")]
        public async Task<IActionResult> ApproveAgentAccess(int userId)
        {
            var result = await uow.UserRepository.ApproveAgentAccess(userId);
            if (result)
            {
                return Ok("Agent access approved successfully.");
            }
            return NotFound("User not found, request not pending, or approval failed.");
        }


        //admin will get agent request
        [HttpGet("users-with-agent-request")]
        public async Task<IActionResult> GetUsersWithAgentRequest()
        {
            var users = await uow.UserRepository.GetUsersWithAgentRequest();
            return Ok(users);
        }



        //Getting all Guest Details
        [HttpGet("get-all-guest")]
        public async Task<IActionResult> GetAllGuestDetails()
        {
            var users = await uow.UserRepository.GetAllGuestUserDetails();
            return Ok(users);
        }


        //Getting all agent detail
        [HttpGet("get-all-agent")]
        public async Task<IActionResult> GetAllAgentDetail()
        {
            var users = await uow.UserRepository.GetAllAgentUserDetails();
            return Ok(users);
        }

        //Getting details of users by Id ___Use this___
        [HttpGet("getuserById/{Id}")]
        public async Task<IActionResult> GetAllDetailsById(int Id)
        {
            var user = await uow.UserRepository.GetDetialById(Id);
            if (user == null)
            {
                return NotFound();
            }
            var userdto = _mapper.Map<UserAgentReqDto>(user);
            return Ok(userdto);
        }

        //Update User Profile
        [HttpPut("updateprofile/{id}")]
        public async Task<IActionResult> UpdateUser(int id, updateProfileDto profileDto)
        {     

            string base64ImageData = profileDto.profile_Img;

            string base64String = base64ImageData.Split(',')[1];
            // Convert base64 image data to byte array
            byte[] bytes = Convert.FromBase64String(base64String);

            string directoryPath = @"D:\CISProject\Assignment Project\HomeRentAngular\src\assets\images\avatar";
            //string directoryPath = @"D:\CISProject\HomeRental\wwwroot\assets\images\avatar";
            string fileName = $"image_{DateTime.Now.Ticks}.png";// Customize the file name and extension as needed
            profileDto.profile_Img = @"assets/images/avatar/" + fileName;

            string filePath = Path.Combine(directoryPath, fileName);

            // Save the image file to the specified path
            System.IO.File.WriteAllBytes(filePath, bytes);

            //fileData
            if (profileDto.profile_Img != null)
            {
                var user = _mapper.Map<Users>(profileDto);
                var updatedProfile = await uow.UserRepository.UpdateUser(id, user);
                var updatedProfileDto = _mapper.Map<updateProfileDto>(updatedProfile);
                return Ok("Success");
            }
            else
            {
                return BadRequest("Invalid source path or file does not exist.");
            }                       
            
        }



        private string CreateJWT(Users users)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            //JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(secretKey));
           /* var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes("thisismycustomSecretkeyforauthentication"));*/

            var claims = new Claim[] { 
             
                new Claim(ClaimTypes.Name,users.UserName),                
                new Claim(ClaimTypes.Email,users.Email),
                new Claim(ClaimTypes.SerialNumber,users.agent.ToString()), 

                new Claim(ClaimTypes.NameIdentifier, users.Id.ToString()),
                new Claim(ClaimTypes.Surname, users.profile_Img.ToString())
                
            };

            var signingCredentials = new SigningCredentials(
                key,SecurityAlgorithms.HmacSha256Signature);

            var takenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = signingCredentials
            };

            //Token Handler this is reponsible to gen JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(takenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}