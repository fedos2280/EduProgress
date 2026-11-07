
using EduProgress.Data;
using EduProgress.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProgress.Controllers
{
    public class LessonsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LessonsController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Відображення уроку
        public async Task<IActionResult> Details(int id)
        {
            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
            {
                return NotFound();
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User);

                ViewBag.IsCompleted =
                    await _context.LessonProgresses.AnyAsync(p =>
                        p.LessonId == id &&
                        p.StudentId == userId &&
                        p.IsCompleted);
            }
            else
            {
                ViewBag.IsCompleted = false;
            }

            return View(lesson);
        }

        // Завершення уроку
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var lessonExists = await _context.Lessons
                .AnyAsync(l => l.Id == id);

            if (!lessonExists)
            {
                return NotFound();
            }

            var progress = await _context.LessonProgresses
                .FirstOrDefaultAsync(p =>
                    p.StudentId == userId &&
                    p.LessonId == id);

            if (progress == null)
            {
                progress = new LessonProgress
                {
                    StudentId = userId,
                    LessonId = id,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow
                };

                _context.LessonProgresses.Add(progress);
            }
            else if (!progress.IsCompleted)
            {
                progress.IsCompleted = true;
                progress.CompletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }
    }
}
