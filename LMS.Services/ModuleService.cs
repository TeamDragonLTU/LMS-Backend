using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.Module;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ModuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        {
            var modules = await _unitOfWork.Modules.GetAllModulesAsync();
            return _mapper.Map<IEnumerable<ModuleDto>>(modules);
        }

        public async Task<ModuleDto?> GetModuleAsync(Guid moduleId, trackChanges: true )
        {
            var module = await _unitOfWork.Modules.GetModuleAsync(moduleId);
            return module == null ? null : _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto)
        {
            var module = _mapper.Map<Domain.Models.Entities.Module>(dto);
            await _unitOfWork.Modules.AddAsync(module);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> UpdateModuleAsync(UpdateModuleDto dto)
        {
            var module = await _unitOfWork.Modules.GetModuleAsync(dto.Id);
            if (module == null)
                throw new ModuleNotFoundException(dto.Id); 
            _mapper.Map(dto, module);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task DeleteModuleAsync(Guid moduleId)
        {
            var module = await _unitOfWork.Modules.GetModuleAsync(moduleId);
            if (module == null)
                throw new ModuleNotFoundException(moduleId); // Skapa denna exception om den saknas
            _unitOfWork.Modules.Remove(module);
            await _unitOfWork.CompleteAsync();
        }
    }
}
