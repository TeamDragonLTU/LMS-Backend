using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Repositories
{
    public  interface IModuleRepository : IRepositoryBase<Module>
    {
        Task<Module?> GetModuleAsync(int moduleId);
        Task<IEnumerable<Module>> GetAllModulesAsync();
    }
}
