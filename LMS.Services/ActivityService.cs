using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.ActivityDtos;
using Service.Contracts;

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
            if (activity == null) throw new ActivityNotFoundException(activityId);
            return _mapper.Map<ActivityDto?>(activity);
        }

        public async Task<IEnumerable<ActivityDto>> GetActivitiesByModuleIdAsync(Guid moduleId)
        {
            var activities = _uow.Activities.GetActivitiesByModuleIdAsync(moduleId);
            return await _mapper.Map<Task<IEnumerable<ActivityDto>>>(activities);
        }

        public async Task DeleteActivityAsync(Guid id)
        {
            var activity = await _uow.Activities.GetActivityByIdAsync(id);
            if (activity == null) throw new ActivityNotFoundException(id);

            _uow.Activities.Delete(activity);
            await _uow.CompleteAsync();
        }

        public async Task<ActivityDto> PostActivityAsync(CreateActivityDto dto)
        {
            var activity = _mapper.Map<Activity>(dto);
            _uow.Activities.Create(activity);
            await _uow.CompleteAsync();
            var activityDto = _mapper.Map<ActivityDto>(activity);
            return activityDto;
        }

        public async Task PutActivityAsync(Guid id, UpdateActivityDto dto)
        {
            var activity = await _uow.Activities.GetActivityByIdAsync(id);
            if (activity == null)
            {
                throw new ActivityNotFoundException(id);
            }
            _mapper.Map(dto, activity);

            _uow.Activities.Update(activity);
            await _uow.CompleteAsync();
        }
    }
}
