using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.Module;
using Service.Contracts;
using System;
using System.Collections.Generic;
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

        public async Task<ModuleDto> GetModuleAsync(Guid moduleId)
        {
            var module = await _unitOfWork.Modules.GetModuleAsync(moduleId);
            if (module == null)
                throw new ModuleNotFoundException(moduleId);

            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto)
        {
            var course = await _unitOfWork.Courses.GetCourseAsync(dto.CourseId);
            if (course == null)
                throw new ArgumentException($"Course with id {dto.CourseId} does not exist.");

            var module = _mapper.Map<Domain.Models.Entities.Module>(dto);
            await _unitOfWork.Modules.AddAsync(module);

            await _unitOfWork.CompleteAsync(); 

            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> UpdateModuleAsync(Guid id, UpdateModuleDto dto)
        {
            var module = await _unitOfWork.Modules.GetModuleAsync(id, trackChanges: true);
            if (module == null)
                throw new ModuleNotFoundException(id);

            _mapper.Map(dto, module);

            await _unitOfWork.CompleteAsync(); 

            return _mapper.Map<ModuleDto>(module);
        }

        public async Task DeleteModuleAsync(Guid moduleId)
        {
            var module = await _unitOfWork.Modules.GetModuleAsync(moduleId);
            if (module == null)
                throw new ModuleNotFoundException(moduleId);

            _unitOfWork.Modules.Remove(module);

            await _unitOfWork.CompleteAsync(); 
        }
    }
}
