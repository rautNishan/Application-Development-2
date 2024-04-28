using System.Net;
using System.Runtime.Serialization;
using CourseWork.Common.Database.Base_Entity;
using CourseWork.Common.Exceptions;
using CourseWork.Modules.Auth.Dtos;
using Microsoft.AspNetCore.Server.HttpSys;

namespace CourseWork.Modules.Auth.Services
{
    public class AuthService
    {
        private readonly ILogger<AuthService> _logger;
        public AuthService(ILogger<AuthService> logger)
        {
            _logger = logger;
        }
        public string Login(BaseUserEntity entity, UserLoginDto incomingData)
        {
            _logger.LogInformation("Incoming Data in AuthService: {Data}", incomingData);
            bool isPasswordCorrect = checkPassword(entity, incomingData.Password);
            if (!isPasswordCorrect)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Invalid Credentials");

            }
            return "akjdhaskdhsakdh";
        }

        public bool checkPassword(BaseUserEntity entity, string incomingPassword)
        {
            if (entity.Password == incomingPassword)
            {
                return true;
            }
            return false;

        }
    }


}
