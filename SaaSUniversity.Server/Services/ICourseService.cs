using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Services
{
    public interface ICourseService
    {
        Task<PagedResult<CourseDto>> GetCoursesCatalogAsync(int pageNumber, int pageSize); Task<bool> EnrollStudentAsync(int studentId, int courseId);
    }
}
