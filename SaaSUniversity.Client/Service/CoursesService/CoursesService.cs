using SaaSUniversity.Client.Service.CoursesService;
using SaaSUniversity.Shared;
using System.Net.Http.Json;

namespace SaaSUniversity.Client.Service.CoursesService
{
    public class CourseService : HttpServiceBase, ICourseService
    {
        public CourseService(HttpClient http) : base(http) { }

        public async Task<PagedResult<CourseDto>?> GetAvailableCoursesAsync(int pageNumber, int pageSize)
        {
            // Build out a safe parameter string target
            var requestUri = $"api/courses?pageNumber={pageNumber}&pageSize={pageSize}";
            var request = CreateAuthenticatedRequest(HttpMethod.Get, requestUri);
            var response = await Http.SendAsync(request);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<PagedResult<CourseDto>>()
                : null;
        }

        public async Task<StudentDto?> GetMyEnrolledCoursesAsync()
        {
            var request = CreateAuthenticatedRequest(HttpMethod.Get, "api/students/my-profile");
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<StudentDto>() : null;
        }

        public async Task<bool> EnrollAsync(int courseId)
        {
            var request = CreateAuthenticatedRequest(HttpMethod.Post, $"api/courses/{courseId}/enroll");
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeregisterAsync(int courseId)
        {
            var request = CreateAuthenticatedRequest(HttpMethod.Delete, $"api/students/{courseId}/deregister");
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
