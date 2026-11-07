
using System.ComponentModel.DataAnnotations;

namespace EduProgress.ViewModels
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Введіть назву курсу")]
        [StringLength(150)]
        [Display(Name = "Назва курсу")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть опис курсу")]
        [StringLength(1000)]
        [Display(Name = "Опис курсу")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть категорію")]
        [StringLength(100)]
        [Display(Name = "Категорія")]
        public string Category { get; set; } = "Веброзробка";

        [Required(ErrorMessage = "Вкажіть рівень")]
        [StringLength(50)]
        [Display(Name = "Рівень складності")]
        public string Level { get; set; } = "Початковий";
    }
}
