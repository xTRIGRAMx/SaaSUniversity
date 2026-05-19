using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            var studentCount = await _context.Students.CountAsync();
            var courseCount = await _context.Courses.CountAsync();

            var enrollmentCount = await _context.Students
                .SelectMany(s => s.Courses)
                .CountAsync();

            var classCount = await _context.Courses
                .SelectMany(c => c.Classes)
                .CountAsync();

            return Ok(new DashboardStatsDto
            {
                StudentCount = studentCount,
                UserCount = studentCount, 
                CourseCount = courseCount,
                EnrollmentCount = enrollmentCount,
                ClassCount = classCount
            });
        }
    }
}