
using System.ComponentModel.DataAnnotations;

namespace EduProgress.ViewModels
{
    public class TestViewModel
    {
        public int AttemptId { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public List<TestQuestionViewModel> Questions { get; set; }
            = new List<TestQuestionViewModel>();
    }

    public class TestQuestionViewModel
    {
        public int Id { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public string OptionA { get; set; } = string.Empty;

        public string OptionB { get; set; } = string.Empty;

        public string OptionC { get; set; } = string.Empty;

        public string OptionD { get; set; } = string.Empty;
    }

    public class TestAnswerViewModel
    {
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Оберіть відповідь")]
        [RegularExpression(
            "^[ABCD]$",
            ErrorMessage = "Некоректний варіант відповіді")]
        public string SelectedAnswer { get; set; } = string.Empty;
    }

    public class TestSubmissionViewModel
    {
        public int AttemptId { get; set; }

        public int CourseId { get; set; }

        public List<TestAnswerViewModel> Answers { get; set; }
            = new List<TestAnswerViewModel>();
    }
}
