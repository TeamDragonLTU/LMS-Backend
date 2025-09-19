using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Activity?> GetActivityById(Guid id)
        {
            return await _context.Activities.FirstOrDefaultAsync(a => a.Id == id);

        }

        //public async Task<IEnumerable<Activity>> GetActivitiesByModuleAsync(int moduleId, bool trackChanges = false)
        //{
        //    return await FindByCondition(a => a.ModuleId == moduleId, trackChanges)
        //                .ToListAsync();
        //}

    }
}
