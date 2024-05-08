﻿using System.Net;
using CourseWork.Common.Dtos;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Blogs.Services;
using CourseWork.Modules.User.Entity;
using CourseWork.Modules.User.Services;
using CourseWork.Modules.Votes.Dtos;
using CourseWork.Modules.Votes.Entity;
using CourseWork.Modules.Votes.Service;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.Votes.Controller
{
    [ApiExplorerSettings(GroupName = "user")]
    [Tags("Votes")]
    [Route("api/user/vote")]
    public class UserVoteController : ControllerBase
    {
        private readonly VoteService _voteService;
        private readonly UserService _userService;

        private readonly BlogService _blogService;

        private readonly ILogger<UserVoteController> _logger;

        public UserVoteController(UserService userService, BlogService blogService, VoteService voteService, ILogger<UserVoteController> logger)
        {
            _userService = userService;
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
            UserEntity? user = await _userService.GetUserByIdAsync(parseUserId);

            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "User not found");
            }
            var userInfo = new CommonUserDto()
            {
                UserId = user.id.ToString(),
                Name = user.UserName
            };

            //Get Blog Info
            BlogEntity? blogInfo = await _blogService.GetByIdAsync(int.Parse(blogId));

            if (blogInfo == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog not found");
            }

            //Check if the user has already voted
            VoteEntity? existingVote = await _voteService.FindVoteByUserAndBlog(blogInfo.id, int.Parse(userInfo.UserId));
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
            UserEntity? user = await _userService.GetUserByIdAsync(parseUserId);

            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "User not found");
            }
            var userInfo = new CommonUserDto()
            {
                UserId = user.id.ToString(),
                Name = user.UserName
            };

            //Get Blog Info
            BlogEntity? blogInfo = await _blogService.GetByIdAsync(int.Parse(blogId));

            if (blogInfo == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog not found");
            }

            //Check if the user has already voted
            VoteEntity? existingVote = await _voteService.FindVoteByUserAndBlog(blogInfo.id, int.Parse(userInfo.UserId));

            _logger.LogInformation("Existing Vote" + existingVote);

            if (existingVote != null) //If the user has already voted
            {
                if (existingVote.IsUpVote == true)
                {
                    //If user has already upvote don't give again
                    throw new HttpException(HttpStatusCode.BadRequest, "You have already down voted");
                }

                if (existingVote.IsUpVote == false)
                {
                    //Initial was upVote now change and it to downvote
                    blogInfo.UpVote += 1;
                    blogInfo.DownVote -= 1;
                    await _blogService.UpdateFormOtherService(blogInfo);
                    existingVote.IsUpVote = true;
                    await _voteService.UpdateVote(existingVote);
                    HttpContext.Items["CustomMessage"] = "Upvote Successfully";
                    return new VoteResponseDto { Id = blogInfo.id };
                }
            }

            //If the user has not voted before
            blogInfo.UpVote += 1;



            //Create Vote
            VoteEntity voteEntity = new VoteEntity()
            {
                BlogId = blogInfo.id,
                IsUpVote = true,
                Blog = blogInfo,
                VoteUser = new UserInfo { UserId = int.Parse(userInfo.UserId), Name = userInfo.Name },

            };

            VoteEntity createdVote = await _voteService.CreateVote(voteEntity);

            if (createdVote == null)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Vote not created");
            }
            _logger.LogInformation("Vote Created" + createdVote);
            //Updating the Blog
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
            UserEntity? user = await _userService.GetUserByIdAsync(parseUserId);

            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Admin not found");
            }
            var userInfo = new CommonUserDto()
            {
                UserId = user.id.ToString(),
                Name = user.UserName
            };

            //Get Blog Info
            BlogEntity? blogInfo = await _blogService.GetByIdAsync(int.Parse(blogId));

            if (blogInfo == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog not found");
            }

            //Check if the user has already voted
            VoteEntity? existingVote = await _voteService.FindVoteByUserAndBlog(blogInfo.id, int.Parse(userInfo.UserId));
            _logger.LogInformation("Existing Vote" + existingVote);
            //Trying to downvote 
            //If it is the user has already up vote now change it to down vote
            if (existingVote != null)
            {
                if (existingVote.IsUpVote == false)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "You have already down voted");
                }

                if (existingVote.IsUpVote == true)
                {
                    //Initial was upVote now change and it to downvote
                    blogInfo.UpVote -= 1;
                    blogInfo.DownVote += 1;
                    await _blogService.UpdateFormOtherService(blogInfo);
                    existingVote.IsUpVote = false;
                    await _voteService.UpdateVote(existingVote);
                    HttpContext.Items["CustomMessage"] = "DownVote Successfully";
                    return new VoteResponseDto { Id = blogInfo.id };

                }
            }

            //If the user has not voted before
            blogInfo.DownVote += 1;
            //Create Vote
            VoteEntity voteEntity = new VoteEntity()
            {
                BlogId = blogInfo.id,
                IsUpVote = false,
                Blog = blogInfo,
                VoteUser = new UserInfo { UserId = int.Parse(userInfo.UserId), Name = userInfo.Name },

            };

            VoteEntity createdVote = await _voteService.CreateVote(voteEntity);

            if (createdVote == null)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Vote not created");
            }
            _logger.LogInformation("Vote Created" + createdVote);
            //Updating the Blog

            blogInfo.Votes.Add(createdVote);
            await _blogService.UpdateFormOtherService(blogInfo);
            HttpContext.Items["CustomMessage"] = "DownVote Successfully";
            VoteResponseDto responseData = new VoteResponseDto { Id = blogInfo.id };
            return responseData;
        }
    }
}
