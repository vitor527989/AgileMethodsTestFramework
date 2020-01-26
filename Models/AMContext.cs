using Microsoft.EntityFrameworkCore;
using AgileMethodsTestFramework.Models;
namespace AgileMethodsTestFramework.Models
{

    public class AMContext : DbContext
    {
        public AMContext(DbContextOptions<AMContext> options) : base(options) { }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QuestionCorrectAnswer> QuestionCorrectAnswers { get; set; }
        public DbSet<QuestionPossibleAnswer> QuestionPossibleAnswers { get; set; }
        public DbSet<ResultTest> ResultsTests { get; set; }
        public DbSet<SubjectUser> SubjectsUsers { get; set; }
        public DbSet<TestAnswer> TestAnswers { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
    }
}