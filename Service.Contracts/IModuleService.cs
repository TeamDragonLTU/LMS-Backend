using LMS.Shared.DTOs.Module;

namespace Service.Contracts
{
    public interface IModuleService
    {
        Task<ModuleDto> GetModuleAsync(Guid moduleId);
        Task<IEnumerable<ModuleDto>> GetAllModulesAsync();
        Task<ModuleDto> PostModuleAsync(CreateModuleDto dto);
        Task PutModuleAsync(Guid id, UpdateModuleDto dto);
        Task DeleteModuleAsync(Guid moduleId);
    }
}
