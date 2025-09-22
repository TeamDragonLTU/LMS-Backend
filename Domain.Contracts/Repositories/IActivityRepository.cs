using Domain.Models.Entities;


namespace Domain.Contracts.Repositories
{
    public interface IActivityRepository : IRepositoryBase<Activity>
    {
        //Task<IEnumerable<Activity>> GetActivitiesByModuleAsync(int moduleId, bool trackChanges = false);

        Task<Activity?> GetActivityByIdAsync(Guid id);
    }
}
