using LMS.Shared.DTOs.ActivityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityDto>> GetActivitiesByModuleIdAsync(Guid moduleId);
        Task<ActivityDto?> GetActivityByIdAsync(Guid activityId);
        Task DeleteActivityAsync(Guid id);

    }
}
