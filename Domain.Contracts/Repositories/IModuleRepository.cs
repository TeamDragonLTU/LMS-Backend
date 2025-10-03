using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    public interface IModuleRepository : IRepositoryBase<Module>
    {
        Task<Module?> GetModuleAsync(Guid moduleId, bool trackChanges = false);
        Task<IEnumerable<Module>> GetAllModulesAsync();
        Task<IEnumerable<Module>> GetModulesByCourseIdAsync(Guid courseId);
        Task AddAsync(Module module);
        void Remove(Module module);
    }
}
