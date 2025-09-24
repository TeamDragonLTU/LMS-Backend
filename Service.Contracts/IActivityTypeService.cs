using LMS.Shared.DTOs.ActivityTypeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IActivityTypeService
    {
        Task<IEnumerable<ActivityTypeDto>> GetAllActivityTypesAsync();
    }
}
