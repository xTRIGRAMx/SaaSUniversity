namespace SaaSUniversity.Server.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;       // e.g. "Lecture 1: Intro to Blazor"
        public string Schedule { get; set; } = string.Empty;   // e.g. "Mondays 10:00 AM"

        // Foreign key to Course
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
