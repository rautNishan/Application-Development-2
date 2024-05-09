using System.Net;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Helper.EmailService;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Modules.User.Dtos;
using CourseWork.Modules.User.Entity;
using CourseWork.Modules.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.User.Controller
{
    [ApiExplorerSettings(GroupName = "user")] //Provides metadata about the API Explorer group that an action belongs to.
    [Tags("Users")]
    [Route("api/user/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;



        public UserController(UserService userService)
        {
            _userService = userService;
        }



        [HttpPost("register")]
        // [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<IActionResult> CreateUser(UserCreateDto incomingData)
        {
            try
            {
                UserEntity result = await _userService.CreateUser(incomingData);
                UserResponseDto responseData = new UserResponseDto { Id = result.id };
                // return Created($"/api/users/{result.Id}", result);
                HttpContext.Items["CustomMessage"] = "Please verify your email to continue";
                return Created("", responseData);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet("info/{userId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]

        public async Task<UserResponseGetById> GetUserById(string userId)
        {
            try
            {
                UserEntity? result = await _userService.GetUserByIdAsync(int.Parse(userId));
                if (result == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Admin not found");
                }

                HttpContext.Items["CustomMessage"] = "User Get Successfully";
                UserResponseGetById dataToSend = new UserResponseGetById
                {
                    Id = result.id,
                    UserName = result.UserName
                };
                return dataToSend;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("soft-delete/{userId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<UserResponseGetById> SoftDeleteUser(string userId)
        {

            UserEntity result = await _userService.SoftDeleteUser(int.Parse(userId));
            return new UserResponseGetById
            {
                Id = result.id,
                UserName = result.UserName
            };

        }

        [HttpPost("restore/{userId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<UserResponseGetById> RestoreUser(string userId)
        {

            UserEntity result = await _userService.RestoreUser(int.Parse(userId));
            return new UserResponseGetById
            {
                Id = result.id,
                UserName = result.UserName
            };
        }

        [HttpDelete("hard-delete/{userId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<UserResponseGetById> HardDelete(string userId)
        {

            UserEntity result = await _userService.HardDelete(int.Parse(userId));
            return new UserResponseGetById
            {
                Id = result.id,
                UserName = result.UserName
            };
        }


        [HttpPost("forget-password")]
        // [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordDto incomingData)
        {
            try
            {
                UserEntity? user = null;

                if (incomingData.Email != null)
                {

                    user = await _userService.FindOneByEmail(incomingData.Email);
                }

                if (user == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "User Not Found");
                }

                await _userService.ForgetPassword(user);
                HttpContext.Items["CustomMessage"] = "Please verify your email to continue";
                return Ok();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("reset-password/{email}")]
        // [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<UserResponseDto> ResetPassword([FromBody] ResetPasswordDot incomingData, string email)
        {
            try
            {
                UserEntity? user = null;

                if (email != null)
                {

                    user = await _userService.FindOneByEmail(email);
                }

                if (user == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "User Not Found");
                }

                UserEntity updatedUser = await _userService.ResetPassword(user, incomingData);
                UserResponseDto dataToResponse = new UserResponseDto() { Id = updatedUser.id };
                HttpContext.Items["CustomMessage"] = "Password Reset Successfully";
                return dataToResponse;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}