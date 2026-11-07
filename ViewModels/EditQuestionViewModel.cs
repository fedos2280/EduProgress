
using System.ComponentModel.DataAnnotations;

namespace EduProgress.ViewModels
{
    public class EditQuestionViewModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        [Required(ErrorMessage = "Введіть текст запитання")]
        [Display(Name = "Текст запитання")]
        public string QuestionText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть варіант A")]
        [Display(Name = "Варіант A")]
        public string OptionA { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть варіант B")]
        [Display(Name = "Варіант B")]
        public string OptionB { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть варіант C")]
        [Display(Name = "Варіант C")]
        public string OptionC { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть варіант D")]
        [Display(Name = "Варіант D")]
        public string OptionD { get; set; } = string.Empty;

        [Required(ErrorMessage = "Оберіть правильну відповідь")]
        [RegularExpression("^[ABCD]$",
            ErrorMessage = "Правильна відповідь: A, B, C або D")]
        [Display(Name = "Правильна відповідь")]
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}
