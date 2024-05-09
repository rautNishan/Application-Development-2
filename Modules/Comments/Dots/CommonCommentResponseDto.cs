namespace CourseWork.Modules.Comments.Dots
{
    public class CommonCommentResponseDto
    {
        public required int Id { get; set; }

        public required int BlogId { get; set; }
        public required string CommentedUserName { get; set; }
        public required string Message { get; set; }
    }
}
