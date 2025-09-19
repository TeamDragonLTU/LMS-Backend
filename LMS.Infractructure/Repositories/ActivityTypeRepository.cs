using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    public class ActivityTypeRepository : RepositoryBase<ActivityType>, IActivityTypeRepository
    {
        public ActivityTypeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ActivityType>> GetAllActivityTypes()
        {
            return await FindAll().ToListAsync();
        }
    }
}
