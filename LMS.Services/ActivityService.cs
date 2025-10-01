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

        public async Task<ActivityDto?> GetActivityByIdAsync(Guid activityId, bool trackChanges = false)
        {
            var activity = await _uow.Activities.GetActivityByIdAsync(activityId, trackChanges);
            if (activity == null) throw new ActivityNotFoundException(activityId);
            return _mapper.Map<ActivityDto?>(activity);
        }

        public async Task<IEnumerable<ActivityDto>> GetActivitiesByModuleIdAsync(Guid moduleId)
        {
            var activities = await _uow.Activities.GetActivitiesByModuleIdAsync(moduleId);
            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
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

            var createdActivity = await _uow.Activities.GetActivityByIdAsync(activity.Id); // To get the ActivityType.Name in the mapping

            var activityDto = _mapper.Map<ActivityDto>(createdActivity);

            return activityDto;
        }

        public async Task PutActivityAsync(Guid id, UpdateActivityDto dto)
        {
            var activity = await _uow.Activities.GetActivityByIdAsync(id, trackChanges: true);

            if (activity == null)
            {
                throw new ActivityNotFoundException(id);
            }

            _mapper.Map(dto, activity);

            try
            {
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                if (!await _uow.Activities.AnyAsync(a => a.Id == id))
                    throw new SaveFailureException("Could not save the activity");
                else
                    throw;
            }
        }
    }
}
