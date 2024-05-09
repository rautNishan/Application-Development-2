namespace CourseWork.Modules.Admin.Dtos
{
    public class AdminCreateResponseDto
    {
        public required int Id { get; set; }
    }

    public class AdminGetByIdResponseDto
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }
    }

    public class AdminPaginatedResponse
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }
    }
}
