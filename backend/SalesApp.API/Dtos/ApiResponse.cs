namespace SalesApp.API.Dtos
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; } = default!;

        public static ApiResponse<T> Success(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Failure(string message)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default!
            };
        }
    }
}