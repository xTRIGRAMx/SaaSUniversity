using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetCoursesCatalogAsync();
        Task<bool> EnrollStudentAsync(int studentId, int courseId);
    }
}
