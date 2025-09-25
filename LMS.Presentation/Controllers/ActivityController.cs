using LMS.Shared.DTOs.ActivityDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;
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

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete activity", Description = "Deletes an activity by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            await _serviceManager.ActivityService.DeleteActivityAsync(id);
            return NoContent();
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Post activity", Description = "Posts an activity")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActivityDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<ActivityDto>> PostActivity(CreateActivityDto dto)
        {
            var activityDto = await _serviceManager.ActivityService.PostActivityAsync(dto);
            return CreatedAtAction(nameof(GetActivityById), new { activityId = activityDto.Id }, activityDto);
        }

    }
}
