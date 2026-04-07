using SaaSUniversity.Server.Models;

namespace SaaSUniversity.Server.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Courses.Any())
            {
                // Courses
                var blazorCourse = new Course { Id = 1, Title = "Intro to Blazor" };
                blazorCourse.Classes.Add(new Class { Id = 1, Name = "Lecture 1", Schedule = "Monday 10 AM" });
                blazorCourse.Classes.Add(new Class { Id = 2, Name = "Lecture 2", Schedule = "Wednesday 2 PM" });

                var dbCourse = new Course { Id = 2, Title = "Database Systems" };
                dbCourse.Classes.Add(new Class { Id = 3, Name = "SQL Basics", Schedule = "Tuesday 9 AM" });
                dbCourse.Classes.Add(new Class { Id = 4, Name = "Database Design", Schedule = "Thursday 11 AM" });

                var cloudCourse = new Course { Id = 3, Title = "Cloud Computing Fundamentals" };
                cloudCourse.Classes.Add(new Class { Id = 5, Name = "Azure Fundamentals", Schedule = "Friday 1 PM" });
                cloudCourse.Classes.Add(new Class { Id = 6, Name = "DevOps Basics", Schedule = "Saturday 9 AM" });

                context.Courses.AddRange(blazorCourse, dbCourse, cloudCourse);

                // Students
                var students = new List<Student>
        {
            new Student { Id = 1, Email = "alice@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!") },
            new Student { Id = 2, Email = "bob@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!") },
            new Student { Id = 3, Email = "charlie@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!") },
            new Student { Id = 4, Email = "diana@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!") }
        };

                context.Students.AddRange(students);

                students[0].Courses.Add(blazorCourse);
                students[1].Courses.Add(dbCourse);

                context.SaveChanges();
            }
        }

    }

}
