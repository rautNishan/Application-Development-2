using System.Net;
using CourseWork.Common.Exceptions;
using CourseWork.Modules.Auth.Dtos;
using CourseWork.Modules.Auth.Services;
using CourseWork.Modules.User.Entity;
using CourseWork.Modules.User.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.Auth.Controllers
{
    [ApiExplorerSettings(GroupName = "user")]
    [Tags("Auth")]
    [Route("api/user/auth/")]
    public class UserAuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;

        private readonly ILogger<UserAuthController> _logger;

        public UserAuthController(AuthService authService, UserService userService, ILogger<UserAuthController> logger)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        async public Task<IActionResult> Login([FromBody] UserLoginDto incomingData)
        {
            try
            {
                UserEntity? user = await _userService.FindOne(incomingData.UserName);
                if (user == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "User Not Found");
                }
                HttpContext.Items["CustomMessage"] = "User LoggedIn Successfully";
                return Ok(_authService.Login(user, incomingData, "user"));

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
