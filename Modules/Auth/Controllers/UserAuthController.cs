using System.Net;
using CourseWork.Common.Exceptions;
using CourseWork.Modules.Auth.Dtos;
using CourseWork.Modules.Auth.Services;
using CourseWork.Modules.User.Dtos;
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
        public async Task<IActionResult> Login([FromBody] UserLoginDto incomingData)
        {
            try
            {
                UserEntity? user = null;

                if (incomingData.UserName != null)
                {
                    user = await _userService.FindOneByUserName(incomingData.UserName);
                }

                if (incomingData.Email != null)
                {

                    user = await _userService.FindOneByEmail(incomingData.Email);
                }

                if (user == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "User Not Found");
                }

                if (user.IsActive == false)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "User Not Found");
                }
                string token = _authService.Login(user, incomingData, "user");
                HttpContext.Items["CustomMessage"] = "User LoggedIn Successfully";
                return Ok(token);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("verify-email/{email}")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            //Decode Email
            email = WebUtility.UrlDecode(email);
            //From Email check that email
            UserEntity? existingUser = await _userService.FindOneByEmail(email);
            if (existingUser == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "User not Found");
            }

            //Update User 
            existingUser.IsActive = true;
            UserEntity updatedData = await _userService.ActivateUser(existingUser);
            UserResponseDto responseData = new UserResponseDto { Id = updatedData.id };
            HttpContext.Items["CustomMessage"] = "User Created Successfully";
            //Redirect user to login page in frontend
            return Redirect("http://localhost:3000/login");
        }

    }
}
