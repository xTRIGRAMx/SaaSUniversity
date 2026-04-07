using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StudentsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<StudentDto>> GetAllStudents()
        {
            var students = await _context.Students
                .Include(s => s.Courses)
                .ThenInclude(c => c.Classes)
                .ToListAsync();

            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                Email = s.Email,
                EnrolledCourses = s.Courses.Select(c => new CourseDto
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
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.Courses)
                .ThenInclude(c => c.Classes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

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
