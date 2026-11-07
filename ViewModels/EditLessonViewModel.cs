
using System.ComponentModel.DataAnnotations;

namespace EduProgress.ViewModels
{
    public class EditLessonViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        [Required(ErrorMessage = "Введіть назву уроку")]
        [StringLength(150)]
        [Display(Name = "Назва уроку")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть навчальний матеріал")]
        [Display(Name = "Навчальний матеріал")]
        public string Content { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "Номер має бути від 1 до 1000")]
        [Display(Name = "Порядковий номер")]
        public int Order { get; set; }
    }
}
