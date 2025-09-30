using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Course?> GetCourseAsync(Guid id, bool trackChanges = false)
        {
            return await FindByCondition(c => c.Id == id, trackChanges).FirstOrDefaultAsync();
        }

        public async Task<int> CourseCountAsync()
        {
            return await FindAll().CountAsync();
        }

        public async Task<Course?> GetCourseWithModulesAndActivitiesAsync(string userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Course)
                    .ThenInclude(c => c.Modules)
                        .ThenInclude(m => m.Activities)
                            .ThenInclude(a => a.ActivityType)
                .Select(u => u.Course)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetCourseParticipantsByUserIdAsync(string userId)
        {
            var courseId = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.CourseId)
                .FirstOrDefaultAsync();

            if (courseId == null)
                return Enumerable.Empty<ApplicationUser>();

            return await _context.Users
                .Where(u => u.CourseId == courseId)
                .ToListAsync();
        }
    }
}
