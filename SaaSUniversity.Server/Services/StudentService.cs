using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        public StudentService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _context.Students
                .Include(s => s.Courses)
                .ThenInclude(c => c.Classes)
                .ToListAsync();

            return students.Select(s => MapToStudentDto(s));
        }

        public async Task<StudentDto?> GetStudentProfileAsync(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Courses)
                .ThenInclude(c => c.Classes)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            return student == null ? null : MapToStudentDto(student);
        }

        public async Task<bool> DeregisterCourseAsync(int studentId, int courseId)
        {
            var student = await _context.Students.Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null) return false;

            var course = student.Courses.FirstOrDefault(c => c.Id == courseId);
            if (course == null) return false;

            student.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        // Helper mapper method to remove repetitive block mapping definitions
        private static StudentDto MapToStudentDto(Models.Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Email = student.Email,
                EnrolledCourses = student.Courses.Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    StudentCount = c.Students.Count,
                    Classes = c.Classes.Select(cls => new ClassDto
                    {
                        Id = cls.Id,
                        Name = cls.Name,
                        Schedule = cls.Schedule
                    }).ToList()
                }).ToList()
            };
        }
    }
}
