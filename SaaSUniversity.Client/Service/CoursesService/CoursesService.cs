using SaaSUniversity.Client.Service.CoursesService;
using SaaSUniversity.Shared;
using System.Net.Http.Json;

namespace SaaSUniversity.Client.Service
{
    public class CourseService : HttpServiceBase, ICourseService
    {
        public CourseService(HttpClient http) : base(http) { }

        public async Task<List<CourseDto>?> GetAvailableCoursesAsync()
        {
            var request = CreateAuthenticatedRequest(HttpMethod.Get, "api/courses");
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<CourseDto>>() : null;
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
