namespace CourseWork.Modules.User.Dtos
{
    public class AuthorizedUserDto
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }

        public required string Email { get; set; }

        public string Name { get; set; }
    }
}
