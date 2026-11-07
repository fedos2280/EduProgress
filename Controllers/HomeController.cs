
using System.Diagnostics;
using EduProgress.Data;
using EduProgress.Models;
using EduProgress.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProgress.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Progress()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }


            var courses = await _context.Courses
                .OrderBy(c => c.Id)
                .ToListAsync();


            var completedLessonIds = await _context.LessonProgresses
                .Where(p =>
                    p.StudentId == userId &&
                    p.IsCompleted)
                .Select(p => p.LessonId)
                .ToListAsync();

            var completedSet = completedLessonIds.ToHashSet();


            var lessons = await _context.Lessons
                .Select(l => new { l.Id, l.CourseId })
                .ToListAsync();

            var progressList = new List<CourseProgressViewModel>();

            foreach (var course in courses)
            {
                var courseLessons = lessons
                    .Where(l => l.CourseId == course.Id)
                    .ToList();

                int totalLessons = courseLessons.Count;

                int completedLessons = courseLessons.Count(l =>
                    completedSet.Contains(l.Id));

                progressList.Add(new CourseProgressViewModel
                {
                    CourseId = course.Id,
                    CourseTitle = course.Title,
                    TotalLessons = totalLessons,
                    CompletedLessons = completedLessons
                });
            }

            return View(progressList);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Teacher()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}
