using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourseWork.Common.database.Base_Model;
using CourseWork.Modules.Comments.Entity;
using CourseWork.Modules.Votes.Entity;

namespace CourseWork.Modules.Blogs.Entity
{
    public class BlogEntity : BaseEntity
    {
        public required string Title { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public required string Content { get; set; }

        public required UserInfo PostUser { get; set; }
        public required string ImgUrl { get; set; }
        public int UpVote { get; set; } = 0;
        public int DownVote { get; set; } = 0;
        public ICollection<VoteEntity> Votes { get; set; } = new List<VoteEntity>();
        public ICollection<CommentsEntity> Comments { get; set; } = new List<CommentsEntity>();


    }
    public class UserInfo()
    {
        public required int UserId { get; set; }
        public required string Name { get; set; }
    }


}
