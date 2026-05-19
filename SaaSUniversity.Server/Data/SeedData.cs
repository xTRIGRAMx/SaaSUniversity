using SaaSUniversity.Server.Models;

namespace SaaSUniversity.Server.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Courses.Any())
            {
                // ==========================================
                // 1. EXTENDED COURSE & CLASS DATA LISTING
                // ==========================================
                var blazorCourse = new Course { Id = 1, Title = "Intro to Blazor & WebAssembly" };
                blazorCourse.Classes.Add(new Class { Id = 1, Name = "Lecture 1: State Management", Schedule = "Monday 10 AM" });
                blazorCourse.Classes.Add(new Class { Id = 2, Name = "Lecture 2: Custom Authentication Providers", Schedule = "Wednesday 2 PM" });

                var dbCourse = new Course { Id = 2, Title = "Database Systems & LINQ Optimization" };
                dbCourse.Classes.Add(new Class { Id = 3, Name = "SQL Basics & Indexing", Schedule = "Tuesday 9 AM" });
                dbCourse.Classes.Add(new Class { Id = 4, Name = "Database Architecture & EF Core Tuning", Schedule = "Thursday 11 AM" });

                var cloudCourse = new Course { Id = 3, Title = "Cloud Computing Fundamentals" };
                cloudCourse.Classes.Add(new Class { Id = 5, Name = "Azure Infrastructure Management", Schedule = "Friday 1 PM" });
                cloudCourse.Classes.Add(new Class { Id = 6, Name = "DevOps Automated Pipelines", Schedule = "Saturday 9 AM" });

                var securityCourse = new Course { Id = 4, Title = "Cybersecurity Essentials & Cryptography" };
                securityCourse.Classes.Add(new Class { Id = 7, Name = "Network Attack Frameworks", Schedule = "Monday 4 PM" });
                securityCourse.Classes.Add(new Class { Id = 8, Name = "Identity Management & Hashing Protocols", Schedule = "Wednesday 9 AM" });

                var aiCourse = new Course { Id = 5, Title = "Artificial Intelligence & Machine Learning" };
                aiCourse.Classes.Add(new Class { Id = 9, Name = "Neural Network Implementations", Schedule = "Tuesday 1 PM" });
                aiCourse.Classes.Add(new Class { Id = 10, Name = "Data Processing Pipelines", Schedule = "Thursday 3 PM" });

                var designCourse = new Course { Id = 6, Title = "Advanced Software Design Patterns" };
                designCourse.Classes.Add(new Class { Id = 11, Name = "Clean Architecture & DDD Principles", Schedule = "Wednesday 11 AM" });
                designCourse.Classes.Add(new Class { Id = 12, Name = "CQRS and Enterprise Patterns", Schedule = "Friday 10 AM" });

                var mobileCourse = new Course { Id = 7, Title = "Mobile Application Development" };
                mobileCourse.Classes.Add(new Class { Id = 13, Name = "Cross-Platform Framework Layouts", Schedule = "Monday 1 PM" });
                mobileCourse.Classes.Add(new Class { Id = 14, Name = "Native Runtime Compilation", Schedule = "Thursday 9 AM" });

                var dsCourse = new Course { Id = 8, Title = "Data Structures & Algorithm Analysis" };
                dsCourse.Classes.Add(new Class { Id = 15, Name = "Asymptotic Notation & Tree Graphs", Schedule = "Tuesday 4 PM" });
                dsCourse.Classes.Add(new Class { Id = 16, Name = "Dynamic Programming Strategies", Schedule = "Friday 3 PM" });

                // Bulk inject courses to seed our multi-page navigation layout grids
                context.Courses.AddRange(
                    blazorCourse, dbCourse, cloudCourse, securityCourse,
                    aiCourse, designCourse, mobileCourse, dsCourse
                );

                // ==========================================
                // 2. EXTENDED MOCK STUDENT ACCOUNTS
                // ==========================================
                var defaultPasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!");

                var students = new List<Student>
                {
                    new Student { Id = 1, Email = "alice@example.com", PasswordHash = defaultPasswordHash },
                    new Student { Id = 2, Email = "bob@example.com", PasswordHash = defaultPasswordHash },
                    new Student { Id = 3, Email = "charlie@example.com", PasswordHash = defaultPasswordHash },
                    new Student { Id = 4, Email = "diana@example.com", PasswordHash = defaultPasswordHash },
                    new Student { Id = 5, Email = "ethan@example.com", PasswordHash = defaultPasswordHash },
                    new Student { Id = 6, Email = "fiona@example.com", PasswordHash = defaultPasswordHash },
                    new Student { Id = 7, Email = "george@example.com", PasswordHash = defaultPasswordHash },
                    new Student { Id = 8, Email = "hannah@example.com", PasswordHash = defaultPasswordHash }
                };

                context.Students.AddRange(students);

                // ==========================================
                // 3. CROSS-RELATIONAL SEED ENROLLMENTS
                // ==========================================
                // Alice joins Blazor and Advanced Software Design Patterns
                students[0].Courses.Add(blazorCourse);
                students[0].Courses.Add(designCourse);

                // Bob joins Database Systems and Mobile Development
                students[1].Courses.Add(dbCourse);
                students[1].Courses.Add(mobileCourse);

                // Charlie joins Cloud Computing and Cybersecurity Essentials
                students[2].Courses.Add(cloudCourse);
                students[2].Courses.Add(securityCourse);

                // Diana joins Artificial Intelligence and Data Structures
                students[3].Courses.Add(aiCourse);
                students[3].Courses.Add(dsCourse);

                // Ethan joins Blazor and Data Structures
                students[4].Courses.Add(blazorCourse);
                students[4].Courses.Add(dsCourse);

                // Fiona joins Database Systems and Cybersecurity Essentials
                students[5].Courses.Add(dbCourse);
                students[5].Courses.Add(securityCourse);

                // Commit everything cleanly to our live database instance mapping layer
                context.SaveChanges();
            }
        }
    }

}
