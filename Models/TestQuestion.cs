
using System.ComponentModel.DataAnnotations;

namespace EduProgress.Models
{
    public class TestQuestion
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public string OptionA { get; set; } = string.Empty;

        [Required]
        public string OptionB { get; set; } = string.Empty;

        [Required]
        public string OptionC { get; set; } = string.Empty;

        [Required]
        public string OptionD { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^[ABCD]$")]
        public string CorrectAnswer { get; set; } = string.Empty;

        public int CourseId { get; set; }

        public Course Course { get; set; } = null!;
    }
}
