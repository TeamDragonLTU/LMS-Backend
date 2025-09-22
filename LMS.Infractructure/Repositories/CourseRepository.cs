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
            return await FindByCondition(m => m.Id == id).FirstOrDefaultAsync();                     
        }
    }
}
