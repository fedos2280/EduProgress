
using System.ComponentModel.DataAnnotations;

namespace EduProgress.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = "Веброзробка";

        public string Level { get; set; } = "Початковий";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
