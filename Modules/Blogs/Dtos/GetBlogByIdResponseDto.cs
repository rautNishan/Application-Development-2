using CourseWork.Modules.Blogs.Entity;

namespace CourseWork.Modules.Blogs.Dtos
{
    public class GetBlogByIdResponseDto
    {
        // Properties from BaseEntity
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Properties from BlogEntity
        public string Title { get; set; }
        public string Content { get; set; }
        public UserInfo PostUser { get; set; }
        public string ImgUrl { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }

        // Structured comments
        public List<CommentDto> Comments { get; set; }
    }
    public class CommentDto
    {
        public int Id { get; set; }
        public string CommentedUserName { get; set; }
        public string Message { get; set; }

        public int? UpVote { get; set; }

        public int? DownVote { get; set; }
        //public List<CommentDto> Replies { get; set; } = new List<CommentDto>();
    }
}
