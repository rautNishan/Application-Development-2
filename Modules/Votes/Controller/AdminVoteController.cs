using System.Net;
using CourseWork.Common.Dtos;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Modules.Admin.Entity;
using CourseWork.Modules.Admin.Services;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Blogs.Services;
using CourseWork.Modules.Votes.Dtos;
using CourseWork.Modules.Votes.Entity;
using CourseWork.Modules.Votes.Service;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.Votes.Controller
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Tags("Votes")]
    [Route("api/admin/vote")]
    public class AdminVoteController : ControllerBase
    {
        private readonly VoteService _voteService;
        private readonly AdminService _adminService;

        private readonly BlogService _blogService;
        private readonly ILogger<AdminVoteController> _logger;

        public AdminVoteController(AdminService adminService, BlogService blogService, VoteService voteService, ILogger<AdminVoteController> logger)
        {
            _adminService = adminService;
            _blogService = blogService;
            _voteService = voteService;
            _logger = logger;
        }

        [HttpPost("info/{blogId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<GetVoteResponseDto?> GetInfoAboutVotes(string blogId)
        {
            //First Get User Info
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

            //Get Blog Info
            BlogEntity? blogInfo = await _blogService.GetByIdAsync(int.Parse(blogId));

            if (blogInfo == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog not found");
            }

            //Check if the user has already voted
            VoteEntity? existingVote = await _voteService.FindVoteByUserAndBlog(blogInfo.id, int.Parse(adminInfo.UserId));
            // if (existingVote != null)
            // {
            //     throw new HttpException(HttpStatusCode.BadRequest, "You have already voted");
            // }
            GetVoteResponseDto returnData = new GetVoteResponseDto()
            {
                Id = existingVote?.id ?? null,
                IsUpVote = existingVote?.IsUpVote ?? null
            };
            return returnData;
        }

        [HttpPost("upvote/{blogId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<VoteResponseDto> UpVote(string blogId)
        {


            //First Get User Info
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

            //Get Blog Info
            BlogEntity? blogInfo = await _blogService.GetByIdAsync(int.Parse(blogId));

            if (blogInfo == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog not found");
            }

            //Check if the user has already voted
            // VoteEntity? existingVote = await _voteService.FindVoteByUserAndBlog(blogInfo.id, int.Parse(adminInfo.UserId));
            // if (existingVote != null)
            // {
            //     throw new HttpException(HttpStatusCode.BadRequest, "You have already voted");
            // }
            //todo

            //Create Vote
            VoteEntity voteEntity = new VoteEntity()
            {
                BlogId = blogInfo.id,
                IsUpVote = true,
                Blog = blogInfo,
                VoteUser = new UserInfo { UserId = int.Parse(adminInfo.UserId), Name = adminInfo.Name },

            };

            VoteEntity createdVote = await _voteService.CreateVote(voteEntity);

            if (createdVote == null)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Vote not created");
            }
            _logger.LogInformation("Vote Created" + createdVote);
            //Updating the Blog
            blogInfo.UpVote += 1;
            blogInfo.Votes.Add(createdVote);
            await _blogService.UpdateFormOtherService(blogInfo);
            HttpContext.Items["CustomMessage"] = "Upvote Successfully";
            VoteResponseDto responseData = new VoteResponseDto { Id = blogInfo.id };
            return responseData;
        }

        [HttpPost("downvote/{blogId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<VoteResponseDto> DownVote(string blogId)
        {


            //First Get User Info
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

            //Get Blog Info
            BlogEntity? blogInfo = await _blogService.GetByIdAsync(int.Parse(blogId));

            if (blogInfo == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog not found");
            }

            //Check if the user has already voted
            // VoteEntity? existingVote = await _voteService.FindVoteByUserAndBlog(blogInfo.id, int.Parse(adminInfo.UserId));
            // if (existingVote != null)
            // {
            //     throw new HttpException(HttpStatusCode.BadRequest, "You have already voted");
            // }
            //todo

            //Create Vote
            VoteEntity voteEntity = new VoteEntity()
            {
                BlogId = blogInfo.id,
                IsUpVote = false,
                Blog = blogInfo,
                VoteUser = new UserInfo { UserId = int.Parse(adminInfo.UserId), Name = adminInfo.Name },

            };

            VoteEntity createdVote = await _voteService.CreateVote(voteEntity);

            if (createdVote == null)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Vote not created");
            }
            _logger.LogInformation("Vote Created" + createdVote);
            //Updating the Blog
            blogInfo.DownVote += 1;
            blogInfo.Votes.Add(createdVote);
            await _blogService.UpdateFormOtherService(blogInfo);
            HttpContext.Items["CustomMessage"] = "DownVote Successfully";
            VoteResponseDto responseData = new VoteResponseDto { Id = blogInfo.id };
            return responseData;
        }



    }
}

