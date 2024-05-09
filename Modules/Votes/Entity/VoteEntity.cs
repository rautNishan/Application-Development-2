using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourseWork.Common.database.Base_Model;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Comments.Entity;

namespace CourseWork.Modules.Votes.Entity
{
    public class VoteEntity : BaseEntity
    {
        public int? BlogId { get; set; }

        [ForeignKey("BlogId")]
        public BlogEntity? Blog { get; set; }
        public UserInfo VoteUser { get; set; }

        public int? CommentsId { get; set; }

        [ForeignKey("CommentsId")]
        public CommentsEntity? Comment { get; set; }
        public bool IsUpVote { get; set; }
    }
}
