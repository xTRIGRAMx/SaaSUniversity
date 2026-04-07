using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSUniversity.Shared
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        // Roll-up field: number of students enrolled
        public int StudentCount { get; set; }

        // Classes inside the course
        public List<ClassDto> Classes { get; set; } = new();
    }
}
