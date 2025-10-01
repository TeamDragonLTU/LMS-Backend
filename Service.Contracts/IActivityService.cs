using LMS.Shared.DTOs.ActivityDtos;

namespace Service.Contracts
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityDto>> GetActivitiesByModuleIdAsync(Guid moduleId);
        Task<ActivityDto?> GetActivityByIdAsync(Guid activityId, bool trackChanges = false);
        Task DeleteActivityAsync(Guid id);
        Task<ActivityDto> PostActivityAsync(CreateActivityDto dto);
        Task PutActivityAsync(Guid activityId, UpdateActivityDto dto);
    }
}
