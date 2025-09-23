using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presentation.Controllers
{
    [Route("api/activity")]
    [ApiController]
    [Produces("application/json")]

    public class ActivityController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ActivityController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("{moduleId}/activities")]
        public async Task<IActionResult> GetActivitiesByModuleId(Guid moduleId)
        {
            var activities = await _serviceManager.ActivityService.GetActivitiesByModuleIdAsync(moduleId);
            return Ok(activities);
        }

        [HttpGet("{activityId}")]
        public async Task<IActionResult> GetActivityById(Guid activityId)
        {
            var activity = await _serviceManager.ActivityService.GetActivityByIdAsync(activityId);
            if (activity == null)
            {
                return NotFound();
            }
            return Ok(activity);
        }
    }
}
