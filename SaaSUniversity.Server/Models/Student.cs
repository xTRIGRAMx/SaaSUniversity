namespace SaaSUniversity.Server.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Many-to-many: a student can enroll in multiple courses
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
