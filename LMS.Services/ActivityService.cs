using AutoMapper;
using Domain.Contracts.Repositories;
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
            return _mapper.Map<ActivityDto?>(activity);
        }

        public async Task<IEnumerable<ActivityDto>> GetActivitiesByModuleIdAsync(Guid moduleId)
        {
            var activities = _uow.Activities.GetActivitiesByModuleIdAsync(moduleId);
            return await _mapper.Map<Task<IEnumerable<ActivityDto>>>(activities);
        }
    }
}
