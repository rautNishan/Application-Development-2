using CourseWork.Common.Dtos;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Votes.Entity;
using CourseWork.Modules.Votes.Repository;

namespace CourseWork.Modules.Votes.Service
{
    public class VoteService
    {
        private readonly VoteRepository _voteRepo;

        public VoteService(VoteRepository voteRepo)
        {
            _voteRepo = voteRepo;
        }

        public async Task<VoteEntity> CreateVote(VoteEntity voteEntity)
        {
            return await _voteRepo.CreateAsync(voteEntity);
        }

        public async Task<VoteEntity?> FindVoteByUserAndBlog(int blogId, int userId)
        {
            return await _voteRepo.FindOne(entity => entity.BlogId == blogId && entity.VoteUser.UserId == userId);
        }

        // public async Task<VoteEntity?> FindVoteByUserAndBlogAndDecideToVote(int blogId, int userId, bool isUpVote)
        // {
        //     return await _voteRepo.FindOne(entity => entity.BlogId == blogId && entity.VoteUser.UserId == userId && entity.IsUpVote == isUpVote);
        // }

        public async Task<VoteEntity> UpdateVote(VoteEntity voteEntity)
        {
            return await _voteRepo.UpdateAsync(voteEntity);
        }

         public async Task<VoteEntity?> FindVoteByUserAndComment(int commentId, int userId)
        {
            return await _voteRepo.FindOne(entity => entity.CommentsId == commentId && entity.VoteUser.UserId == userId);
        }
    }
}
