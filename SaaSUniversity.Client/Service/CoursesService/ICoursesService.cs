using SaaSUniversity.Shared;

namespace SaaSUniversity.Client.Service.CoursesService
{
    public interface ICourseService
    {
        Task<PagedResult<CourseDto>?> GetAvailableCoursesAsync(int pageNumber, int pageSize); Task<StudentDto> GetMyEnrolledCoursesAsync();
        Task<bool> EnrollAsync(int courseId);
        Task<bool> DeregisterAsync(int courseId);
    }
}
