using LMS.Shared.DTOs.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IModuleService
    {
        Task<ModuleDto?> GetModuleAsync(int moduleId);
        Task<IEnumerable<ModuleDto>> GetAllModulesAsync();
    }
}
