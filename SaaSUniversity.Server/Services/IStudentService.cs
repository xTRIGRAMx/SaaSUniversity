using SaaSUniversity.Shared;

namespace SaaSUniversity.Server.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentProfileAsync(int studentId);
        Task<bool> DeregisterCourseAsync(int studentId, int courseId);
    }
}
