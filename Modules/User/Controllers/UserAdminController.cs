using System.Net;
using CourseWork.Common.Exceptions;
using CourseWork.Modules.User.Dtos;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Modules.User.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CourseWork.Modules.User.Entity;
using CourseWork.Common.Middlewares.Response;
using CourseWork.Common.Constants.Enums;
using CourseWork.Modules.Admin.Entity;

namespace CourseWork.Modules.User.Controller
{

    [ApiExplorerSettings(GroupName = "admin")] //Provides metadata about the API Explorer group that an action belongs to.
    [Tags("Users")]
    [Route("api/admin/user")]
    [ApiController]
    public class UserAdminsController : ControllerBase
    {
        private readonly UserService _userService;
        public UserAdminsController(UserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        [ServiceFilter(typeof(RoleAuthFilter))]

        public async Task<IActionResult> CreateUser(UserCreateDto incomingData)
        {
            try
            {
                var result = await _userService.CreateUser(incomingData);

                HttpContext.Items["CustomMessage"] = "User Created Successfully";

                return Created("", result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("list")]
        [ServiceFilter(typeof(RoleAuthFilter))]

        public async Task<PaginatedResponse<UserResponseGetById>> ListAdmin([FromQuery] string? page = "1", [FromQuery] ShortByEnum shortBy = ShortByEnum.Latest)
        {
            try
            {
                int parsePage = int.Parse(page);
                PaginatedResponse<UserEntity> result = await _userService.GetPaginatedUserList(parsePage, shortBy);
                PaginatedResponse<UserResponseGetById> mappedResults = new PaginatedResponse<UserResponseGetById>
                {
                    PageNumber = result.PageNumber,
                    DataPerPage = result.DataPerPage,
                    TotalCount = result.TotalCount,
                    Data = result.Data.Select(user => new UserResponseGetById
                    {
                        Id = user.id,
                        UserName = user.Name
                    })
                };
                HttpContext.Items["CustomMessage"] = "Admin List Successfully";
                return mappedResults;
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

        [HttpPatch("update/{userId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]

        public async Task<UserResponseGetById> CreateAdmin(UserUpdateDto incomingData, string userId)
        {
            try
            {
                UserEntity? result = await _userService.GetUserByIdAsync(int.Parse(userId));

                if (result == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "User not found");
                }
                UserEntity updatedResult = await _userService.UpdateUser(result, incomingData);
                UserResponseGetById dataToSend = new UserResponseGetById
                {
                    Id = updatedResult.id,
                    UserName = updatedResult.UserName
                };
                HttpContext.Items["CustomMessage"] = "User Updated Successfully";
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

    }
}
