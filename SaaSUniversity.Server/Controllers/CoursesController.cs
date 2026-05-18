using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSUniversity.Server.Extensions;
using SaaSUniversity.Server.Services;
using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService) => _courseService = courseService;

        [HttpGet]
        public async Task<IEnumerable<CourseDto>> GetCourses()
        {
            return await _courseService.GetCoursesCatalogAsync();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("{courseId}/enroll")]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var studentId = User.GetStudentId();
            if (studentId == null) return Unauthorized();

            var success = await _courseService.EnrollStudentAsync(studentId.Value, courseId);
            return success ? Ok() : NotFound();
        }
    }
}
