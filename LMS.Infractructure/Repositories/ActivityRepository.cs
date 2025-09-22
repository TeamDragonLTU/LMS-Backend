using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Activity?> GetActivityByIdAsync(Guid id)
        {
            return await FindAll().FirstOrDefaultAsync(a => a.Id == id);

        }

        public async Task<IEnumerable<Activity>> GetActivitiesByModuleIdAsync(Guid moduleId)
        {
            return await FindByCondition(a => a.ModuleID == moduleId)
                        .ToListAsync();
        }

    }
}
