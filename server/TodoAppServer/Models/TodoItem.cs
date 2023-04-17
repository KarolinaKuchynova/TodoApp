using System.ComponentModel.DataAnnotations;

namespace TodoAppServer.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string? Name { get; set; }

        [Required]
        public int? Priority { get; set; }

        [Required]
        public TodoItemStatus? Status { get; set; }
    }

    public enum TodoItemStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
}
