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
    }
}