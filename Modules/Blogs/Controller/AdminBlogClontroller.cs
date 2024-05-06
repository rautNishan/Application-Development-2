using System.Net;
using CourseWork.Common.Dtos;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Modules.Admin.Dtos;
using CourseWork.Modules.Admin.Entity;
using CourseWork.Modules.Admin.Services;
using CourseWork.Modules.Blogs.Dtos;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Blogs.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.Blogs.Controller
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Tags("Blogs")]
    [Route("api/admin/blogs")]
    public class AdminBlogController : ControllerBase
    {
        private readonly BlogService _blogService;
        private readonly ILogger<AdminBlogController> _logger;

        private readonly AdminService _adminService;

        public AdminBlogController(BlogService blogService, ILogger<AdminBlogController> logger, AdminService adminService)
        {
            _blogService = blogService;
            _logger = logger;
            _adminService = adminService;
        }

        [HttpPost("create")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<IActionResult> CreateBlogs([FromBody] BlogCreateDto incomingData)
        {
            string userId = (HttpContext.Items["UserId"] as string)!; //Since we are using the RoleAuthFilter, we can safely assume that the UserId is a string and never null
            int parseUserId = int.Parse(userId); // Convert the string to an int
            AdminEntity? adminUser = await _adminService.GetUserByIdAsync(parseUserId);

            if (adminUser == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Admin not found");
            }
            var adminInfo = new CommonUserDto()
            {
                UserId = adminUser.id.ToString(),
                Name = adminUser.UserName
            };
            BlogEntity result = await _blogService.CreateBlogs(incomingData, adminInfo);
            return Created("", result);
        }

    }
}
