using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using CourseWork.Common.Database.Base_Entity;
using CourseWork.Common.Exceptions;
using CourseWork.Modules.Auth.Dtos;
using Microsoft.IdentityModel.Tokens;


namespace CourseWork.Modules.Auth.Services
{
    public class AuthService
    {


        private const string SecretKey = "your_secret_key_here_ahdkahdkjashksaaskh_Nishan_Raut_hdkjashdkjsakdhsa_Raut_Nishan";
        private readonly ILogger<AuthService> _logger;
        public AuthService(ILogger<AuthService> logger)
        {
            _logger = logger;

        }

        public string Login(BaseUserEntity entity, UserLoginDto incomingData, string role)
        {

            try
            {
                bool isPasswordCorrect = checkPassword(entity, incomingData.Password);
                if (!isPasswordCorrect)
                {
                    throw new HttpException(HttpStatusCode.Unauthorized, "Invalid Credentials");
                }

                // Define const Key this should be private secret key  stored in some safe place
                return GenerateToken(entity.id.ToString(), role);
            }
            catch (Exception)
            {
                _logger.LogError("Error in AuthService.Login");
                throw;
            }
        }

        public bool checkPassword(BaseUserEntity entity, string incomingPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(incomingPassword, entity.Password);
            }
            catch (Exception ex)
            {

                _logger.LogError("Error in AuthService.checkPassword" + ex);
                throw;
            }
        }
        private string GenerateToken(string userId, string role)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("userId", userId),
                new Claim("role", role)
            }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }


}
