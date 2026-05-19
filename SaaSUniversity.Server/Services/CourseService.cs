using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaaSUniversity.Server.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        public CourseService(AppDbContext context) => _context = context;

        public async Task<PagedResult<CourseDto>> GetCoursesCatalogAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalCount = await _context.Courses.CountAsync();

                var courses = await _context.Courses
                    .Include(c => c.Classes)
                    .Include(c => c.Students)
                    .OrderBy(c => c.Title) 
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var mappedItems = courses.Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    StudentCount = c.Students.Count,
                    Classes = c.Classes.Select(cls => new ClassDto
                    {
                        Id = cls.Id,
                        Name = cls.Name,
                        Schedule = cls.Schedule
                    }).ToList()
                }).ToList();

                // 3. Package it all inside the generic wrapper
                return new PagedResult<CourseDto>
                {
                    Items = mappedItems,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database failure in GetCoursesCatalogAsync: {ex.Message}");

                return new PagedResult<CourseDto>
                {
                    Items = new List<CourseDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            try
            {
                var student = await _context.Students.Include(s => s.Courses)
                    .FirstOrDefaultAsync(s => s.Id == studentId);

                var course = await _context.Courses.Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == courseId);

                if (student == null || course == null) return false;

                if (student.Courses.Any(c => c.Id == courseId))
                {
                    return false;
                }

                student.Courses.Add(course);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database failure in EnrollStudentAsync: {ex.Message}");

                return false;
            }
        }
    }
}