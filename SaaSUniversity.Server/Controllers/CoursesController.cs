using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Server.Models;
using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CoursesController(AppDbContext context) => _context = context;


        [HttpGet]
        public async Task<IEnumerable<CourseDto>> GetCourses()
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

        [HttpPost("{courseId}/enroll/{studentId}")]
        public async Task<IActionResult> Enroll(int courseId, int studentId)
        {
            var student = await _context.Students.Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == studentId);
            var course = await _context.Courses.FindAsync(courseId);

            if (student == null || course == null) return NotFound();

            student.Courses.Add(course);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{courseId}/deregister/{studentId}")]
        public async Task<IActionResult> Deregister(int courseId, int studentId)
        {
            var student = await _context.Students.Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null) return NotFound();

            var course = student.Courses.FirstOrDefault(c => c.Id == courseId);
            if (course != null) student.Courses.Remove(course);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
