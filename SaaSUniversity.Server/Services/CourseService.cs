using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        public CourseService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<CourseDto>> GetCoursesCatalogAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.Classes)
                .Include(c => c.Students)
                .ToListAsync();

            return courses.Select(c => new CourseDto
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
            });
        }

        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            var student = await _context.Students.Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            var course = await _context.Courses.Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (student == null || course == null) return false;

            if (!student.Courses.Any(c => c.Id == courseId))
            {
                student.Courses.Add(course);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
