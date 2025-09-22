using Domain.Models.Entities;


namespace Domain.Contracts.Repositories
{
    public interface IActivityRepository : IRepositoryBase<Activity>
    {
        Task<IEnumerable<Activity>> GetActivitiesByModuleIdAsync(Guid moduleId);

        Task<Activity?> GetActivityByIdAsync(Guid id);
    }
}
