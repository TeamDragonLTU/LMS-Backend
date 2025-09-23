using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
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

        public ActivityService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(Guid activityId)
        {
            var activity = await _uow.Activities.GetActivityByIdAsync(activityId);
            return _mapper.Map<ActivityDto?>(activity);
        }

        // GetActivitiesByModuleIdAsync does not work until ModuleId uncommented in Activity
        public async Task<IEnumerable<ActivityDto>> GetActivitiesByModuleIdAsync(Guid moduleId)
        {
            var activities = _uow.Activities.GetActivitiesByModuleIdAsync(moduleId);
            return await _mapper.Map<Task<IEnumerable<ActivityDto>>>(activities);
        }

        //anropas från controllern
        //Får en createActivityDto därifrån
        // GÖr om den till en Activity entitet
        //Säger till repository, via uow, att spara den
        public async Task<ActivityDto> PostActivityAsync(CreateActivityDto dto)
        {
            var activity = _mapper.Map<Activity>(dto);
            _uow.Activities.Create(activity);
            await _uow.CompleteAsync();
            var activityDto = _mapper.Map<ActivityDto>(activity);
            return activityDto;
        }
    }
}
