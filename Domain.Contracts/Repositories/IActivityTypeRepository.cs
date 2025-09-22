using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    public interface IActivityTypeRepository : IRepositoryBase<ActivityType>
    {

        Task<IEnumerable<ActivityType>> GetAllActivityTypesAsync();
    }
}
