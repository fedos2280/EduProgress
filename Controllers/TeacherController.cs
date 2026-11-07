
using EduProgress.Data;
using EduProgress.Models;
using EduProgress.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProgress.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .ToListAsync();

            return View(courses);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View(new CreateCourseViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(
            CreateCourseViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var course = new Course
            {
                Title = model.Title.Trim(),
                Description = model.Description.Trim(),
                Category = model.Category.Trim(),
                Level = model.Level.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Курс успішно створено!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CreateLesson(int courseId)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return NotFound();

            int lastOrder = await _context.Lessons
                .Where(l => l.CourseId == courseId)
                .Select(l => (int?)l.Order)
                .MaxAsync() ?? 0;

            ViewBag.CourseTitle = course.Title;

            return View(new CreateLessonViewModel
            {
                CourseId = courseId,
                Order = lastOrder + 1
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLesson(
            CreateLessonViewModel model)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == model.CourseId);

            if (course == null)
                return NotFound();

            ViewBag.CourseTitle = course.Title;

            bool orderExists = await _context.Lessons
                .AnyAsync(l =>
                    l.CourseId == model.CourseId &&
                    l.Order == model.Order);

            if (orderExists)
            {
                ModelState.AddModelError(
                    nameof(model.Order),
                    "Урок із таким номером уже існує.");
            }

            if (!ModelState.IsValid)
                return View(model);

            var lesson = new Lesson
            {
                CourseId = model.CourseId,
                Title = model.Title.Trim(),
                Content = model.Content.Trim(),
                Order = model.Order
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Урок успішно додано!";

            return RedirectToAction(
                "Details", "Courses",
                new { id = model.CourseId });
        }

        [HttpGet]
        public async Task<IActionResult> CreateQuestion(int courseId)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return NotFound();

            ViewBag.CourseTitle = course.Title;

            return View(new CreateTestQuestionViewModel
            {
                CourseId = courseId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(
            CreateTestQuestionViewModel model)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == model.CourseId);

            if (course == null)
                return NotFound();

            ViewBag.CourseTitle = course.Title;

            if (!ModelState.IsValid)
                return View(model);

            var question = new TestQuestion
            {
                CourseId = model.CourseId,
                QuestionText = model.QuestionText.Trim(),
                OptionA = model.OptionA.Trim(),
                OptionB = model.OptionB.Trim(),
                OptionC = model.OptionC.Trim(),
                OptionD = model.OptionD.Trim(),
                CorrectAnswer = model.CorrectAnswer
            };

            _context.TestQuestions.Add(question);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Тестове запитання успішно додано!";

            return RedirectToAction(nameof(Index));
            
        }
        
[HttpGet]
public async Task<IActionResult> Results()
{
    var attempts = await _context.TestAttempts
        .AsNoTracking()
        .Include(a => a.Student)
        .Include(a => a.Course)
        .Where(a => a.IsCompleted)
        .OrderByDescending(a => a.CompletedAt)
        .ToListAsync();

    return View(attempts);
}

        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var model = new EditCourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Category = course.Category,
                Level = course.Level
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(
            int id, EditCourseViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            course.Title = model.Title.Trim();
            course.Description = model.Description.Trim();
            course.Category = model.Category.Trim();
            course.Level = model.Level.Trim();

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Курс успішно оновлено!";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> EditLesson(int id)
        {
            var lesson = await _context.Lessons
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == lesson.CourseId);

            ViewBag.CourseTitle = course?.Title ?? "Курс";

            return View(new EditLessonViewModel
            {
                Id = lesson.Id,
                CourseId = lesson.CourseId,
                Title = lesson.Title,
                Content = lesson.Content,
                Order = lesson.Order
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLesson(
            int id, EditLessonViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
            {
                return NotFound();
            }

            if (lesson.CourseId != model.CourseId)
            {
                return BadRequest();
            }

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == lesson.CourseId);

            ViewBag.CourseTitle = course?.Title ?? "Курс";

            bool orderExists = await _context.Lessons
                .AnyAsync(l =>
                    l.CourseId == lesson.CourseId &&
                    l.Order == model.Order &&
                    l.Id != lesson.Id);

            if (orderExists)
            {
                ModelState.AddModelError(
                    nameof(model.Order),
                    "Урок із таким номером уже існує.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            lesson.Title = model.Title.Trim();
            lesson.Content = model.Content.Trim();
            lesson.Order = model.Order;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Урок успішно оновлено!";

            return RedirectToAction(
                "Details",
                "Courses",
                new { id = lesson.CourseId });
        }



        
        [HttpGet]
        public async Task<IActionResult> EditQuestion(int id)
        {
            var question = await _context.TestQuestions
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            return View(new EditQuestionViewModel
            {
                Id = question.Id,
                CourseId = question.CourseId,
                QuestionText = question.QuestionText,
                OptionA = question.OptionA,
                OptionB = question.OptionB,
                OptionC = question.OptionC,
                OptionD = question.OptionD,
                CorrectAnswer = question.CorrectAnswer
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(
            int id, EditQuestionViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var question = await _context.TestQuestions
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            if (question.CourseId != model.CourseId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            question.QuestionText = model.QuestionText.Trim();
            question.OptionA = model.OptionA.Trim();
            question.OptionB = model.OptionB.Trim();
            question.OptionC = model.OptionC.Trim();
            question.OptionD = model.OptionD.Trim();
            question.CorrectAnswer = model.CorrectAnswer;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Запитання успішно оновлено!";

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Questions()
        {
            var questions = await _context.TestQuestions
                .AsNoTracking()
                .Include(q => q.Course)
                .OrderBy(q => q.CourseId)
                .ThenBy(q => q.Id)
                .ToListAsync();

            return View(questions);
        }


    }

    
   
}

