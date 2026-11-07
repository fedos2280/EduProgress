
using System.ComponentModel.DataAnnotations;

namespace EduProgress.Models
{
    public class LessonProgress
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public Lesson Lesson { get; set; } = null!;

        [Required]
        [StringLength(450)]
        public string StudentId { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        public DateTime? CompletedAt { get; set; }
    }
}
