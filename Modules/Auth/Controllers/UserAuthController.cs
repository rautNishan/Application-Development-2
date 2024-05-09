using System.Net;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Helper.EmailService;
using CourseWork.Common.Middlewares.Auth;
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

        private readonly EmailService _emailService;
        private readonly ILogger<UserAuthController> _logger;

        public UserAuthController(AuthService authService, UserService userService, ILogger<UserAuthController> logger, EmailService emailService)
        {
            _authService = authService;
            _userService = userService;
            _emailService = emailService;
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


        [HttpGet("verify-password-change/{email}")]
        async public Task<IActionResult> ChangePassword(string email)
        {
            //Decode Email
            email = WebUtility.UrlDecode(email);
            //From Email check that email
            UserEntity? existingUser = await _userService.FindOneByEmail(email);
            if (existingUser == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "User not Found");
            }
            HttpContext.Items["CustomMessage"] = "Change You Password";
            //Redirect user to login page in frontend
            return Redirect("http://localhost:3000/reset-password");
        }

        [HttpPost("change-password")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<UserResponseDto> ChangePassword([FromBody] ChangePasswordDto incomingData)
        {
            try
            {

                string userId = (HttpContext.Items["UserId"] as string)!; //Since we are using the RoleAuthFilter, we can safely assume that the UserId is a string and never null
                int parseUserId = int.Parse(userId); // Convert the string to an int
                UserEntity? user = await _userService.GetUserByIdAsync(parseUserId);

                if (user == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Admin not found");
                }
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(incomingData.OldPassword);
                bool isPasswordCorrect = _authService.checkPassword(user, hashedPassword);
                if (!isPasswordCorrect)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "Old Password is not correct");
                }
                UserEntity updatedUser = await _userService.ChangePassword(user, incomingData);
                UserResponseDto dataToResponse = new UserResponseDto() { Id = updatedUser.id };
                HttpContext.Items["CustomMessage"] = "Password Changed Successfully";
                return dataToResponse;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}