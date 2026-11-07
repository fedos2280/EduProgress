
using System.ComponentModel.DataAnnotations;

namespace EduProgress.Models
{
    public class TestAnswer
    {
        public int Id { get; set; }

        public int TestAttemptId { get; set; }

        public TestAttempt TestAttempt { get; set; } = null!;

        public int TestQuestionId { get; set; }

        public TestQuestion TestQuestion { get; set; } = null!;

        [Required]
        [RegularExpression("^[ABCD]$")]
        public string SelectedAnswer { get; set; } = string.Empty;


        public bool IsCorrect { get; set; }
    }
}
