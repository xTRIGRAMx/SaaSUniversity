using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSUniversity.Shared
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public List<CourseDto> EnrolledCourses { get; set; } = new();
    }
}
