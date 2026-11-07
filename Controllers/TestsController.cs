
using EduProgress.Data;
using EduProgress.Models;
using EduProgress.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EduProgress.Controllers
{
    [Authorize(Roles = "Student")]
    public class TestsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TestsController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Take(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            var questions = await _context.TestQuestions
                .AsNoTracking()
                .Where(q => q.CourseId == id)
                .OrderBy(q => q.Id)
                .Select(q => new TestQuestionViewModel
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    OptionA = q.OptionA,
                    OptionB = q.OptionB,
                    OptionC = q.OptionC,
                    OptionD = q.OptionD
                })
                .ToListAsync();

            if (questions.Count == 0)
                return BadRequest("У цьому курсі немає запитань.");

            var attempt = await _context.TestAttempts
                .Where(a =>
                    a.StudentId == userId &&
                    a.CourseId == id &&
                    !a.IsCompleted)
                .OrderBy(a => a.Id)
                .FirstOrDefaultAsync();

            int attemptsCount = await _context.TestAttempts
                .CountAsync(a =>
                    a.StudentId == userId &&
                    a.CourseId == id);

            if (attempt == null)
            {
                if (attemptsCount >= 3)
                    return BadRequest("Усі 3 спроби використано.");

                attempt = new TestAttempt
                {
                    StudentId = userId,
                    CourseId = id,
                    StartedAt = DateTime.UtcNow,
                    IsCompleted = false
                };

                _context.TestAttempts.Add(attempt);
                await _context.SaveChangesAsync();
            }

            return View(new TestViewModel
            {
                AttemptId = attempt.Id,
                CourseId = course.Id,
                CourseTitle = course.Title,
                Questions = questions
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(
            TestSubmissionViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var attempt = await _context.TestAttempts
                .AsNoTracking()
                .FirstOrDefaultAsync(a =>
                    a.Id == model.AttemptId &&
                    a.StudentId == userId &&
                    a.CourseId == model.CourseId);

            if (attempt == null)
                return NotFound();

            if (attempt.IsCompleted)
                return RedirectToAction(
                    nameof(Result),
                    new { id = attempt.Id });

            var questions = await _context.TestQuestions
                .AsNoTracking()
                .Where(q => q.CourseId == attempt.CourseId)
                .ToListAsync();

            if (questions.Count == 0 ||
                model.Answers == null ||
                model.Answers.Count != questions.Count)
            {
                return BadRequest(
                    "Потрібно відповісти на всі запитання.");
            }

            var submittedIds = model.Answers
                .Select(a => a.QuestionId)
                .ToList();

            if (submittedIds.Distinct().Count() != questions.Count ||
                submittedIds.Except(questions.Select(q => q.Id)).Any() ||
                model.Answers.Any(a =>
                    a.SelectedAnswer == null ||
                    !new[] { "A", "B", "C", "D" }
                        .Contains(a.SelectedAnswer)))
            {
                return BadRequest("Некоректні відповіді тесту.");
            }

            int correctCount = model.Answers.Count(answer =>
            {
                var question = questions.First(q =>
                    q.Id == answer.QuestionId);

                return answer.SelectedAnswer == question.CorrectAnswer;
            });

            int score = (int)Math.Round(
                correctCount * 100.0 / questions.Count);

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.ReadCommitted);


            int updated = await _context.TestAttempts
                .Where(a =>
                    a.Id == attempt.Id &&
                    a.StudentId == userId &&
                    a.CourseId == model.CourseId &&
                    !a.IsCompleted)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.IsCompleted, true)
                    .SetProperty(a => a.CorrectAnswers, correctCount)
                    .SetProperty(a => a.TotalQuestions, questions.Count)
                    .SetProperty(a => a.ScorePercent, score)
                    .SetProperty(a => a.CompletedAt, DateTime.UtcNow));

            if (updated == 0)
            {
                await transaction.RollbackAsync();

                return RedirectToAction(
                    nameof(Result),
                    new { id = attempt.Id });
            }

            foreach (var answer in model.Answers)
            {
                var question = questions.First(q =>
                    q.Id == answer.QuestionId);

                _context.TestAnswers.Add(new TestAnswer
                {
                    TestAttemptId = attempt.Id,
                    TestQuestionId = question.Id,
                    SelectedAnswer = answer.SelectedAnswer,
                    IsCorrect =
                        answer.SelectedAnswer == question.CorrectAnswer
                });
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return RedirectToAction(
                nameof(Result),
                new { id = attempt.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Result(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var attempt = await _context.TestAttempts
                .AsNoTracking()
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a =>
                    a.Id == id &&
                    a.StudentId == userId &&
                    a.IsCompleted);

            if (attempt == null)
                return NotFound();

            return View(new TestResultViewModel
            {
                AttemptId = attempt.Id,
                CourseId = attempt.CourseId,
                CourseTitle = attempt.Course.Title,
                CorrectAnswers = attempt.CorrectAnswers,
                TotalQuestions = attempt.TotalQuestions,
                ScorePercent = attempt.ScorePercent,
                CompletedAt = attempt.CompletedAt
            });
        }
    }
}
