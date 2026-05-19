using System.ComponentModel.DataAnnotations;

namespace SaaSUniversity.Shared
{
    public class StudentDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; } = string.Empty;

        public List<CourseDto> EnrolledCourses { get; set; } = new();
    }
}
