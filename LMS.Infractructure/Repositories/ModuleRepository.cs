using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
    {

        public ModuleRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Module?> GetModuleAsync(Guid moduleId)
        {
            return await FindAll().FirstOrDefaultAsync(m => m.Id == moduleId);
        }

        public async Task<IEnumerable<Module>> GetAllModulesAsync()
        {
            return await FindAll().Include(m => m.Course).ToListAsync();
        }

        public async Task AddAsync(Module module)
        {
            Create(module);
            await Task.CompletedTask;
        }

        public void Remove(Module module)
        {
            Delete(module);
        }
    }
}
