namespace CourseWork.Modules.Blogs.Dtos
{
    public class BlogUpdateDto
    {

        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string ImgUrl { get; set; }
        // public required int UpVote { get; set; }
        // public required int DownVote { get; set; }
    }
}
