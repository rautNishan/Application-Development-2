namespace CourseWork.Common.Middlewares.Dtos
{
    public record ErrorResponseDto
    {
        public DateTime Date { get; set; }

        public required string RequestedUrl { get; set; }
        public required string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
