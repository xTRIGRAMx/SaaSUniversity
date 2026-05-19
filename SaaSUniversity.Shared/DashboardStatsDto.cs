namespace SaaSUniversity.Shared
{
    public class DashboardStatsDto
    {
        public int TenantCount { get; set; } = 1; // Baseline fallback metric
        public int UserCount { get; set; }
        public int StudentCount { get; set; }
        public int EnrollmentCount { get; set; }
        public int ClassCount { get; set; }
        public int CourseCount { get; set; }
        public int InstructorCount { get; set; } = 4; // Baseline fallback metric
    }
}
