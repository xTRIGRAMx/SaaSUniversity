using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Server.Models;
using SaaSUniversity.Server.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SaaSUniversity.Tests
{
    public class CourseServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly CourseService _service;

        public CourseServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new CourseService(_context);
        }

        [Fact]
        public async Task GetCoursesCatalogAsync_ShouldReturnCorrectlyPaginatedData()
        {
            //Seed database
            for (int i = 1; i <= 7; i++)
            {
                _context.Courses.Add(new Course { Id = i, Title = $"Course {i}" });
            }
            await _context.SaveChangesAsync();

            var result = await _service.GetCoursesCatalogAsync(pageNumber: 1, pageSize: 5);

            Assert.NotNull(result);
            Assert.Equal(5, result.Items.Count);
            Assert.Equal(7, result.TotalCount);
            Assert.Equal(2, result.TotalPages);
            Assert.True(result.HasNextPage); 
            Assert.False(result.HasPreviousPage);   
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}