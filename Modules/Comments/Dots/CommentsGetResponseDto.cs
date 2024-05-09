namespace CourseWork.Modules.Comments.Dots
{
    public class CommentsGetResponseDto
    {
        public int Id { get; set; }
        public string CommentedUserName { get; set; }
        public string Message { get; set; }
        public List<ReplyDto> Replies { get; set; } = new List<ReplyDto>();
    }

    public class ReplyDto
    {
        public string Message { get; set; }
        public string CommentedUserName { get; set; }
    }
}
