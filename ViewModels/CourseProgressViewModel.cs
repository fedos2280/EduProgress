
namespace EduProgress.ViewModels
{
    public class CourseProgressViewModel
    {
        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public int TotalLessons { get; set; }

        public int CompletedLessons { get; set; }

        public int ProgressPercent
        {
            get
            {
                if (TotalLessons == 0)
                {
                    return 0;
                }

                return (int)Math.Round(
                    CompletedLessons * 100.0 / TotalLessons);
            }
        }
    }
}
