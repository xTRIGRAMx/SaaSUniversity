namespace SaaSUniversity.Server.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        // Many-to-many: a course can have multiple students
        public ICollection<Student> Students { get; set; } = new List<Student>();

        // One-to-many: a course can have multiple classes
        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
