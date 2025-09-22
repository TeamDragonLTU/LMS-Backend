using LMS.Shared.DTOs.CourseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API
{
    [Route("api/course")]
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
        [SwaggerOperation(Summary = "Get all courses", Description = "Gets all courses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses()
        {
            return Ok(await _serviceManager.CourseService.GetAllCoursesAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get course by ID", Description = "Returns a specific course")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CourseDto>> GetCourse(Guid id)
        {
            return Ok(await _serviceManager.CourseService.GetMovieAsync(id));
        }

    }
}
