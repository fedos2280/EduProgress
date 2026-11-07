
using EduProgress.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduProgress.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; } = null!;

        public DbSet<Lesson> Lessons { get; set; } = null!;

        public DbSet<LessonProgress> LessonProgresses { get; set; } = null!;

        public DbSet<TestQuestion> TestQuestions { get; set; } = null!;

        public DbSet<TestAttempt> TestAttempts { get; set; } = null!;

        public DbSet<TestAnswer> TestAnswers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LessonProgress>()
                .HasIndex(p => new { p.StudentId, p.LessonId })
                .IsUnique();

            modelBuilder.Entity<TestQuestion>()
                .HasOne(q => q.Course)
                .WithMany()
                .HasForeignKey(q => q.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TestAttempt>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TestAttempt>()
                .HasOne(a => a.Course)
                .WithMany()
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TestAnswer>()
                .HasOne(a => a.TestAttempt)
                .WithMany()
                .HasForeignKey(a => a.TestAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TestAnswer>()
                .HasOne(a => a.TestQuestion)
                .WithMany()
                .HasForeignKey(a => a.TestQuestionId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TestAnswer>()
                .HasIndex(a => new
                {
                    a.TestAttemptId,
                    a.TestQuestionId
                })
                .IsUnique();
        }
    }
}
