using SaaSUniversity.Shared;

namespace SaaSUniversity.Client.Service.CoursesService
{
    public interface ICourseService
    {
        Task<List<CourseDto>?> GetAvailableCoursesAsync();
        Task<StudentDto> GetMyEnrolledCoursesAsync();
        Task<bool> EnrollAsync(int courseId);
        Task<bool> DeregisterAsync(int courseId);
    }
}
