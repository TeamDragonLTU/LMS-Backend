using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace LMS.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CourseController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CourseController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            return Ok(await _serviceManager.CourseService.GetAllCoursesAsync(trackChanges: false));
        }

    }
}
