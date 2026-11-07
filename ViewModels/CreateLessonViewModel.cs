
using System.ComponentModel.DataAnnotations;

namespace EduProgress.ViewModels
{
    public class CreateLessonViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Введіть назву уроку")]
        [StringLength(150)]
        [Display(Name = "Назва уроку")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть матеріал уроку")]
        [Display(Name = "Навчальний матеріал")]
        public string Content { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "Номер має бути від 1 до 1000")]
        [Display(Name = "Порядковий номер уроку")]
        public int Order { get; set; } = 1;
    }
}
