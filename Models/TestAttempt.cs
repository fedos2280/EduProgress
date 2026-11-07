
namespace EduProgress.Models
{
    public class TestAttempt
    {
        public int Id { get; set; }

        public string StudentId { get; set; } = string.Empty;

        public ApplicationUser Student { get; set; } = null!;

        public int CourseId { get; set; }

        public Course Course { get; set; } = null!;


        public int CorrectAnswers { get; set; }

        public int TotalQuestions { get; set; }

        public int ScorePercent { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
