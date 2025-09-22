using AutoMapper;
using Domain.Contracts.Repositories;
using LMS.Shared.DTOs.ActivityDtos;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ActivityService(IUnitOfWork uow, IMapper mapper) { 
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(Guid activityId)
        {
           return await _mapper.Map<Task<ActivityDto?>>(
                _uow.Activities.GetActivityByIdAsync(activityId)
            );
        }

        public async Task<IEnumerable<ActivityDto>> GetActivitiesByModuleIdAsync(Guid moduleId)
        {
            return await _mapper.Map<Task<IEnumerable<ActivityDto>>>(_uow.Activities.GetActivitiesByModuleIdAsync(moduleId));
        }
    }
}
