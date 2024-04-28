using System.Runtime.InteropServices;

namespace CourseWork.Common.Middlewares.Dtos
{
    public record ResponseDto
    {
        public DateTime Date { get; set; }
        public required string RequestedUrl { get; set; }

        public string Message { get; set; }
        public int StatusCode { get; set; }

        public required object Data { get; set; }

    }
}
