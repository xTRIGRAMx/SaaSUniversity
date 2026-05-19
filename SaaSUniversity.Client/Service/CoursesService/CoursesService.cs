using SaaSUniversity.Client.Service.CoursesService;
using SaaSUniversity.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SaaSUniversity.Client.Service.CoursesService
{
    public class CourseService : HttpServiceBase, ICourseService
    {
        public CourseService(HttpClient http) : base(http) { }

        public async Task<DashboardStatsDto?> GetDashboardStatsAsync()
        {
            try
            {
                var request = CreateAuthenticatedRequest(HttpMethod.Get, "api/dashboard/stats");
                var response = await Http.SendAsync(request);

                return response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<DashboardStatsDto>()
                    : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Network error fetching metric parameters: {ex.Message}");
                return null; 
            }
        }

        public async Task<PagedResult<CourseDto>?> GetAvailableCoursesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var requestUri = $"api/courses?pageNumber={pageNumber}&pageSize={pageSize}";
                var request = CreateAuthenticatedRequest(HttpMethod.Get, requestUri);
                var response = await Http.SendAsync(request);

                return response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<PagedResult<CourseDto>>()
                    : GetEmptyFallback(pageNumber, pageSize);
            }
            catch (Exception ex)
            {           
                Console.WriteLine($"Network error fetching course listings: {ex.Message}");
                return GetEmptyFallback(pageNumber, pageSize);
            }
        }

        public async Task<StudentDto?> GetMyEnrolledCoursesAsync()
        {
            try
            {
                var request = CreateAuthenticatedRequest(HttpMethod.Get, "api/students/my-profile");
                var response = await Http.SendAsync(request);
                return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<StudentDto>() : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Network error fetching active student profile links: {ex.Message}");
                return null; 
            }
        }

        public async Task<bool> EnrollAsync(int courseId)
        {
            try
            {
                var request = CreateAuthenticatedRequest(HttpMethod.Post, $"api/courses/{courseId}/enroll");
                var response = await Http.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Network error attempting registration update: {ex.Message}");
                return false; 
            }
        }

        public async Task<bool> DeregisterAsync(int courseId)
        {
            try
            {
                var request = CreateAuthenticatedRequest(HttpMethod.Delete, $"api/students/{courseId}/deregister");
                var response = await Http.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Network error attempting profile catalog change: {ex.Message}");
                return false; 
            }
        }

        private static PagedResult<CourseDto> GetEmptyFallback(int pageNumber, int pageSize)
        {
            return new PagedResult<CourseDto>
            {
                Items = new List<CourseDto>(),
                TotalCount = 0,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}