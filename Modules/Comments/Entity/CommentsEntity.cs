using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourseWork.Common.database.Base_Model;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Votes.Entity;

namespace CourseWork.Modules.Comments.Entity
{
    public class CommentsEntity : BaseEntity
    {

        [Required]
        public int CommentedUserId { get; set; }

        [Required]
        public required string CommentedUserName { get; set; }
        [Required]
        public required string Message { get; set; }

        public int? UpVote { get; set; } = 0;
        public int? DownVote { get; set; } = 0;
        public int BlogId { get; set; }

        [ForeignKey("BlogId")]
        public BlogEntity? BlogEntity { get; set; }

        public int? ParentCommentId { get; set; }

        [ForeignKey("ParentCommentId")]
        public CommentsEntity? ParentComment { get; set; }

        public ICollection<VoteEntity> Votes { get; set; } = new List<VoteEntity>();
    }
}
