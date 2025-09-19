using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Activity?> GetActivityById(Guid id)
        {
            return await FindAll().FirstOrDefaultAsync(a => a.Id == id);

        }

        //public async Task<IEnumerable<Activity>> GetActivitiesByModuleAsync(int moduleId, bool trackChanges = false)
        //{
        //    return await FindByCondition(a => a.ModuleId == moduleId, trackChanges)
        //                .ToListAsync();
        //}

    }
}
