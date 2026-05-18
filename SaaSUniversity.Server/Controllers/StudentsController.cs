using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSUniversity.Server.Extensions; // 👈 Gives access to .GetStudentId()
using SaaSUniversity.Server.Services;
using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService) => _studentService = studentService;

        [HttpGet]
        public async Task<IEnumerable<StudentDto>> GetAllStudents()
        {
            return await _studentService.GetAllStudentsAsync();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("my-profile")]
        public async Task<ActionResult<StudentDto>> GetStudent()
        {
            var studentId = User.GetStudentId();
            if (studentId == null) return Unauthorized("Session expired.");

            var student = await _studentService.GetStudentProfileAsync(studentId.Value);
            return student == null ? NotFound() : Ok(student);
        }

        // 🔑 Configured to use your requested /api/Students/{courseId}/deregister routing structure
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpDelete("{courseId}/deregister")]
        public async Task<IActionResult> Deregister(int courseId)
        {
            var studentId = User.GetStudentId();
            if (studentId == null) return Unauthorized();

            var success = await _studentService.DeregisterCourseAsync(studentId.Value, courseId);
            return success ? Ok() : NotFound();
        }
    }
}
