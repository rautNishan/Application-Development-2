using CourseWork.Modules.Blogs.Entity;

namespace CourseWork.Modules.Votes.Dtos
{
    public record VoteCreateDto
    {
        public int BlogId { get; init; }
        public bool IsUpVote { get; init; }
    }
}
