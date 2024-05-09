using System.Net;
using CourseWork.Common.Dtos;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Blogs.Services;
using CourseWork.Modules.Comments.Dots;
using CourseWork.Modules.Comments.Entity;
using CourseWork.Modules.Comments.Services;
using CourseWork.Modules.User.Entity;
using CourseWork.Modules.User.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.Comments.Controllers
{
    [ApiExplorerSettings(GroupName = "user")]
    [Tags("Comments")]
    [Route("api/user/blogs")]
    public class UserCommentsController : ControllerBase
    {
        private readonly CommentsService _commentsService;

        private readonly BlogService _blogService;

        private readonly UserService _userService;

        public UserCommentsController(CommentsService commentsService, BlogService blogService, UserService userService)
        {
            _commentsService = commentsService;
            _blogService = blogService;
            _userService = userService;
        }

        [HttpPost("create/{blogId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<CommonCommentResponseDto> CreateComment([FromBody] CommentCreateDto incomingData, string blogId)
        {
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
                Name = user.Name
            };

            BlogEntity? existingBlog = await _blogService.GetByIdAsync(int.Parse(blogId));
            if (existingBlog == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog not found");
            }


            CommentsEntity result = await _commentsService.CreateCommentAsync(incomingData, existingBlog, userInfo);
            CommonCommentResponseDto dataToResponse = new CommonCommentResponseDto()
            {
                Id = result.id,
                BlogId = result.BlogId,
                CommentedUserName = result.CommentedUserName,
                Message = result.Message,

            };

            HttpContext.Items["CustomMessage"] = "Comment Created Successfully";
            return dataToResponse;

        }

        [HttpPost("update/{commentId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<CommonCommentResponseDto> UpdateComment([FromBody] UpdateCommentDto incomingData, string commentId)
        {
            string userId = (HttpContext.Items["UserId"] as string)!; //Since we are using the RoleAuthFilter, we can safely assume that the UserId is a string and never null
            int parseUserId = int.Parse(userId); // Convert the string to an int
            UserEntity? user = await _userService.GetUserByIdAsync(parseUserId);

            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "User not found");
            }

            //If User its User Comment then he can update it
            CommentsEntity? existingComment = await _commentsService.GetByIdAsync(int.Parse(commentId));

            if (existingComment == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Comment not found");
            }

            //Check if the user is the owner of the comment
            if (existingComment.CommentedUserId != user.id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "Sorry Cannot Edit Others Comment");
            }

            //if own then get the to edit the comment
            CommentsEntity updatedComment = await _commentsService.UpdateComments(existingComment, incomingData);

            CommonCommentResponseDto dataToResponse = new CommonCommentResponseDto()
            {
                Id = updatedComment.id,
                BlogId = updatedComment.BlogId,
                CommentedUserName = updatedComment.CommentedUserName,
                Message = updatedComment.Message,

            };

            return dataToResponse;

        }

        [HttpPost("reply/{commentId}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<CommonCommentResponseDto> ReplyComment([FromBody] CommentCreateDto incomingData, string commentId)
        {
            string userId = (HttpContext.Items["UserId"] as string)!; //Since we are using the RoleAuthFilter, we can safely assume that the UserId is a string and never null
            int parseUserId = int.Parse(userId); // Convert the string to an int
            UserEntity? user = await _userService.GetUserByIdAsync(parseUserId);

            //Check if User is Null or Not
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "User not found");
            }

            //Create User Information
            CommonUserDto userInfo = new CommonUserDto()
            {
                UserId = user.id.ToString(),
                Name = user.Name
            };

            //Check if the Parent Comment Exists or not
            CommentsEntity? parentComment = await _commentsService.GetByIdAsync(int.Parse(commentId));

            if (parentComment == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Comment not found");
            }

            //Check if the Blog Exists of not
            BlogEntity? existingBlog = await _blogService.GetByIdAsync(parentComment.BlogId);
            if (existingBlog == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog not found");
            }

            CommentsEntity result = await _commentsService.ReplyCommentAsync(incomingData, parentComment, userInfo);

            CommonCommentResponseDto dataToResponse = new CommonCommentResponseDto()
            {
                Id = result.id,
                BlogId = result.BlogId,
                CommentedUserName = result.CommentedUserName,
                Message = result.Message,

            };

            HttpContext.Items["CustomMessage"] = "Comment Reply Successfully";
            return dataToResponse;

        }

        [HttpGet("comment/{comment}")]
        [ServiceFilter(typeof(RoleAuthFilter))]

        public async Task<CommentsGetResponseDto> GetCommentsById(string comment)
        {
            CommentsGetResponseDto? commentDto = await _commentsService.GetCommentWithReplies(int.Parse(comment));
            if (commentDto == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Comment with that id was not found");
            }
            return commentDto;
        }
    }
}
