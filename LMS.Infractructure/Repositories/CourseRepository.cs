using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Course?> GetCourseAsync(Guid id)
        {
            return await FindByCondition(c => c.Id == id).FirstOrDefaultAsync();                     
        }

        public async Task<bool> AnyCourseAsync(Guid id)
        {
            return await FindAll().AnyAsync(c => c.Id == id);
        }

        public async Task<int> CourseCountAsync()
        {
            return await FindAll().CountAsync();
        }
    }
}
