namespace CourseWork.Modules.User.Dtos
{
    public class UserResponseDto
    {
        public required int Id { get; set; }

    }

    public class UserResponseGetById{
        public required int Id { get; set; }
        public required string UserName { get; set; }
    }
}
