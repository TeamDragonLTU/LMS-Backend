using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ModuleDto?> GetModuleAsync(Guid moduleId, bool trackChanges = false)
        {
            var module = await _unitOfWork.Modules.GetModuleAsync(moduleId, trackChanges);
            return module == null ? null : _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto)
        {
            var course = await _unitOfWork.Courses.GetCourseAsync(dto.CourseId);
            if (course == null)
                throw new ArgumentException($"Course with id {dto.CourseId} does not exist.");

            var module = _mapper.Map<Domain.Models.Entities.Module>(dto);
            await _unitOfWork.Modules.AddAsync(module);
            var changes = await SaveChangesAsync();
            if (changes <= 0)
                throw new SaveFailureException("Could not save the module");
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> UpdateModuleAsync(Guid id, UpdateModuleDto dto)
        {
            var module = await _unitOfWork.Modules.GetModuleAsync(id, trackChanges: true);
            if (module == null)
                throw new ModuleNotFoundException(dto.Id); 
            _mapper.Map(dto, module);
            var changes = await SaveChangesAsync();
            if (changes <= 0)
                throw new SaveFailureException("Could not save the module update");
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task DeleteModuleAsync(Guid moduleId)
        {
            var module = await _unitOfWork.Modules.GetModuleAsync(moduleId);
            if (module == null)
                throw new ModuleNotFoundException(moduleId);
            _unitOfWork.Modules.Remove(module);
            var changes = await SaveChangesAsync();
            if (changes <= 0)
                throw new SaveFailureException("Could not delete the module");
        }

        // Helper för att få antal ändringar från SaveChangesAsync
        private async Task<int> SaveChangesAsync()
        {
            var contextProp = _unitOfWork.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (contextProp?.GetValue(_unitOfWork) is DbContext context)
                return await context.SaveChangesAsync();
            await _unitOfWork.CompleteAsync(); // fallback, men returnerar inget
            return 1; // antag att det lyckades om vi inte kan få context
        }
    }
}
