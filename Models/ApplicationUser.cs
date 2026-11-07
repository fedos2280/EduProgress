
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EduProgress.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; }
            = DateTime.UtcNow;
    }
}
