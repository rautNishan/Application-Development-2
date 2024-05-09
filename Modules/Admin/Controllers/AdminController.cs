using System.Net;
using CourseWork.Common.Constants.Enums;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Common.Middlewares.Response;
using CourseWork.Modules.Admin.Dtos;
using CourseWork.Modules.Admin.Entity;
using CourseWork.Modules.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")] //Provides metadata about the API Explorer group that an action belongs to.
    [Tags("Admin")]
    [Route("api/admin/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;
        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("register")]
        [ServiceFilter(typeof(RoleAuthFilter))]

        public async Task<IActionResult> CreateAdmin(AdminCreateDto incomingData)
        {
            try
            {
                AdminEntity result = await _adminService.RegisterAdmin(incomingData);

                HttpContext.Items["CustomMessage"] = "User Created Successfully";
                AdminCreateResponseDto dataToSend = new AdminCreateResponseDto
                {
                    Id = result.id
                };
                return Created("", dataToSend);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("info/{adminId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]

        public async Task<AdminGetByIdResponseDto> GetAdminById(string adminId)
        {
            try
            {
                AdminEntity? result = await _adminService.GetUserByIdAsync(int.Parse(adminId));
                if (result == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Admin not found");
                }

                HttpContext.Items["CustomMessage"] = "Admin Created Successfully";
                AdminGetByIdResponseDto dataToSend = new AdminGetByIdResponseDto
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

        [HttpGet("list")]
        [ServiceFilter(typeof(RoleAuthFilter))]

        public async Task<PaginatedResponse<AdminPaginatedResponse>> ListAdmin([FromQuery] string? page = "1", [FromQuery] ShortByEnum shortBy = ShortByEnum.Latest)
        {
            try
            {
                int parsePage = int.Parse(page);
                PaginatedResponse<AdminEntity> result = await _adminService.GetPaginatedBlogList(parsePage, shortBy);
                PaginatedResponse<AdminPaginatedResponse> mappedResults = new PaginatedResponse<AdminPaginatedResponse>
                {
                    PageNumber = result.PageNumber,
                    DataPerPage = result.DataPerPage,
                    TotalCount = result.TotalCount,
                    Data = result.Data.Select(admin => new AdminPaginatedResponse
                    {
                        Id = admin.id,
                        UserName = admin.UserName
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

        [HttpPost("update/{adminId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]

        public async Task<AdminCreateResponseDto> CreateAdmin(AdminUpdateDto incomingData, string adminId)
        {
            try
            {
                AdminEntity? result = await _adminService.GetUserByIdAsync(int.Parse(adminId));

                if (result == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Admin not found");
                }
                AdminEntity updatedResult = await _adminService.UpdateAdmin(result, incomingData);
                AdminCreateResponseDto dataToSend = new AdminCreateResponseDto
                {
                    Id = updatedResult.id
                };
                HttpContext.Items["CustomMessage"] = "Admin Updated Successfully";
                return dataToSend;

            }
            catch (Exception)
            {
                throw;
            }


        }

        [HttpDelete("soft-delete/{adminId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<AdminCreateResponseDto> SoftDeleteBlog(string adminId)
        {

            AdminEntity result = await _adminService.SoftDeleteBlog(int.Parse(adminId));
            return new AdminCreateResponseDto
            {
                Id = result.id
            };

        }

        [HttpPost("restore/{adminId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<AdminCreateResponseDto> RestoreBlog(string adminId)
        {

            AdminEntity result = await _adminService.RestoreBlog(int.Parse(adminId));
            return new AdminCreateResponseDto
            {
                Id = result.id
            };
        }

        [HttpDelete("hard-delete/{adminId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<AdminCreateResponseDto> HardDelete(string adminId)
        {

            AdminEntity result = await _adminService.HardDelete(int.Parse(adminId));
            return new AdminCreateResponseDto
            {
                Id = result.id
            };
        }
    }
}
