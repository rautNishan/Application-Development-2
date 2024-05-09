using System.Net;
using CourseWork.Common.Dtos;
using CourseWork.Common.Exceptions;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Comments.Dots;
using CourseWork.Modules.Comments.Entity;
using CourseWork.Modules.Comments.Repository;

namespace CourseWork.Modules.Comments.Services
{
    public class CommentsService
    {
        private readonly CommentsRepository _commentsRepo;
        public CommentsService(CommentsRepository commentsRepo)
        {
            _commentsRepo = commentsRepo;
        }


        public async Task<CommentsEntity> CreateCommentAsync(CommentCreateDto commentCreateDto, BlogEntity blog, CommonUserDto userInfo)
        {
            CommentsEntity comment = new CommentsEntity
            {

                CommentedUserId = int.Parse(userInfo.UserId),
                CommentedUserName = userInfo.Name,
                Message = commentCreateDto.Message,
                BlogId = blog.id,
                BlogEntity = blog,
                ParentCommentId = null,
                ParentComment = null
            };
            await _commentsRepo.CreateAsync(comment);
            return comment;
        }

        public async Task<CommentsEntity> ReplyCommentAsync(CommentCreateDto commentReplyDto, CommentsEntity parentComment, CommonUserDto userInfo)
        {
            CommentsEntity comment = new CommentsEntity
            {

                CommentedUserId = int.Parse(userInfo.UserId),
                CommentedUserName = userInfo.Name,
                Message = commentReplyDto.Message,
                BlogId = parentComment.BlogId,
                BlogEntity = parentComment.BlogEntity,
                ParentCommentId = parentComment.id,
                ParentComment = parentComment
            };
            await _commentsRepo.CreateAsync(comment);
            return comment;
        }

        public async Task<CommentsEntity> UpdateComments(CommentsEntity commentsEntity, UpdateCommentDto incomingData)
        {
            if (incomingData.Message != null)
            {
                commentsEntity.Message = incomingData.Message;
            }
            return await _commentsRepo.UpdateAsync(commentsEntity);
        }


        public async Task<CommentsEntity?> GetByIdAsync(int id)
        {
            return await _commentsRepo.FindByIdAsync(id);

        }

        public async Task<CommentsGetResponseDto> GetCommentWithReplies(int commentId)
        {
            var (parentComment, replies) = await _commentsRepo.GetCommentWithRepliesAsync(commentId);

            if (parentComment == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Comment not found");
            }

            CommentsGetResponseDto result = new CommentsGetResponseDto
            {
                Id = parentComment.id,
                CommentedUserName = parentComment.CommentedUserName,
                Message = parentComment.Message,
                Replies = replies.Select(r => new ReplyDto
                {
                    Message = r.Message,
                    CommentedUserName = r.CommentedUserName
                }).ToList()
            };

            return result;
        }
    }
}
