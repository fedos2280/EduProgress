
namespace EduProgress.ViewModels
{
    public class TestResultViewModel
    {
        public int AttemptId { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public int CorrectAnswers { get; set; }

        public int TotalQuestions { get; set; }

        public int ScorePercent { get; set; }

        public DateTime? CompletedAt { get; set; }

        public bool IsPassed => ScorePercent >= 60;
    }
}
