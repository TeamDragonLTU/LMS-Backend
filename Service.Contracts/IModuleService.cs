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
        Task<ModuleDto?> GetModuleAsync(Guid moduleId);
        Task<IEnumerable<ModuleDto>> GetAllModulesAsync();
        Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto);
        Task<ModuleDto> UpdateModuleAsync(UpdateModuleDto dto);
        Task DeleteModuleAsync(Guid moduleId);
    }
}
