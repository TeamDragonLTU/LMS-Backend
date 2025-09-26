using LMS.Shared.DTOs.CourseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [SwaggerOperation(Summary = "Get all courses", Description = "Gets all courses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses()
        {
            return Ok(await _serviceManager.CourseService.GetAllCoursesAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get course by ID", Description = "Returns a specific course")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CourseDto>> GetCourse(Guid id)
        {
            return Ok(await _serviceManager.CourseService.GetCourseAsync(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(Summary = "Update course", Description = "Updates an existing course by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> PutCourse(Guid id, [FromBody] UpdateCourseDto dto)
        {
            await _serviceManager.CourseService.PutCourseAsync(id, dto);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(Summary = "Create course", Description = "Creates a new course.", Tags = ["Course"])]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CourseDto>> PostCourse(CreateCourseDto dto)
        {
            var courseDto = await _serviceManager.CourseService.PostCourseAsync(dto);
            return CreatedAtAction(nameof(GetCourse), new { id = courseDto.Id }, courseDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(Summary = "Delete course", Description = "Deletes a course by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            await _serviceManager.CourseService.DeleteCourseAsync(id);
            return NoContent();
        }

    }
}
