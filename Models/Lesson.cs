
using System.ComponentModel.DataAnnotations;

namespace EduProgress.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int Order { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; } = null!;
    }
}
