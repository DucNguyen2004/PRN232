using System.Text.Json.Serialization;

namespace DTOs
{
    public class ApiResponseDto<T>
    {
        public string Status { get; set; } = "success";

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ErrorDetails? Error { get; set; }
    }

    public class ErrorDetails
    {
        public required string Title { get; set; }
        public required int StatusCode { get; set; }
        public required string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InnerMessage { get; set; }
    }
}
