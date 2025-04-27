using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public string UserId { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}
