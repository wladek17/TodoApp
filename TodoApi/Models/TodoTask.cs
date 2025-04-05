namespace TodoApi.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
