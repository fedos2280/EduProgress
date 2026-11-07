
using EduProgress.Data;
using EduProgress.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProgress.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .OrderBy(c => c.Id)
                .ToListAsync();

            return View(courses);
        }


        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }


            var lessons = await _context.Lessons
                .Where(l => l.CourseId == id)
                .OrderBy(l => l.Order)
                .ToListAsync();

            ViewBag.Lessons = lessons;

            int totalLessons = lessons.Count;
            int completedLessons = 0;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User);

                if (!string.IsNullOrEmpty(userId))
                {
                    completedLessons = await _context.LessonProgresses
                        .Where(p =>
                            p.StudentId == userId &&
                            p.IsCompleted &&
                            p.Lesson.CourseId == id)
                        .CountAsync();
                }
            }

            int progressPercent = totalLessons == 0
                ? 0
                : (int)Math.Round(
                    completedLessons * 100.0 / totalLessons);

            ViewBag.TotalLessons = totalLessons;
            ViewBag.CompletedLessons = completedLessons;
            ViewBag.ProgressPercent = progressPercent;

            return View(course);
        }
    }
}
