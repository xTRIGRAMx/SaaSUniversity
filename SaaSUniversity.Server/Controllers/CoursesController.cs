using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSUniversity.Server.Extensions;
using SaaSUniversity.Server.Services;
using SaaSUniversity.Shared;
using System;
using System.Threading.Tasks;

namespace SaaSUniversity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService) => _courseService = courseService;

        [HttpGet]
        public async Task<ActionResult<PagedResult<CourseDto>>> GetCourses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 5;

                var result = await _courseService.GetCoursesCatalogAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Controller Exception in GetCourses: {ex.Message}");
                return StatusCode(500, "An internal error occurred while fetching the course catalog.");
            }
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("{courseId}/enroll")]
        public async Task<IActionResult> Enroll(int courseId)
        {
            try
            {
                var studentId = User.GetStudentId(); //
                if (studentId == null) return Unauthorized(); //

                var success = await _courseService.EnrollStudentAsync(studentId.Value, courseId); //

                return success ? Ok() : BadRequest(new { Message = "Enrollment rejected. Course might be full, user already enrolled, or entity not found." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Controller Exception in Enroll: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your enrollment request.");
            }
        }
    }
}