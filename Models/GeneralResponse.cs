namespace backend_II_Exercises.Models
{
    public class GeneralResponse<T>
    {
        public string Code { get; set; } = "0";
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}