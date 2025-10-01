using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext context) : base(context) { }


        public async Task<Activity?> GetActivityByIdAsync(Guid id, bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Include(a => a.ActivityType)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Activity>> GetActivitiesByModuleIdAsync(Guid ModuleId)
        {
            return await FindByCondition(a => a.ModuleId == ModuleId)
                .Include(a => a.ActivityType)
                .ToListAsync();
        }

    }
}
