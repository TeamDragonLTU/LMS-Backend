using LMS.Shared.DTOs.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMS.Infractructure.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace LMS.API
{
    [Route("api/classmates")]
    [ApiController]
    [Authorize]
    public class ClassmatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Domain.Models.Entities.ApplicationUser> _userManager;
        public ClassmatesController(ApplicationDbContext context, UserManager<Domain.Models.Entities.ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetClassmates([FromQuery] string? courseId = null)
        {
            Guid? resolvedCourseGuid = null;
            if (!string.IsNullOrEmpty(courseId))
            {
                if (!Guid.TryParse(courseId, out var parsedGuid))
                    return BadRequest("Invalid courseId format");
                resolvedCourseGuid = parsedGuid;
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                    return Unauthorized();

                var user = await _context.Users.Include(u => u.Course).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null || user.CourseId == null)
                    return NotFound("User or course not found");
                resolvedCourseGuid = user.CourseId;
            }

            var classmatesRaw = await _context.Users
                .Where(u => u.CourseId == resolvedCourseGuid)
                .ToListAsync();

            var classmates = new List<(UserDto user, string role)>();
            foreach (var u in classmatesRaw)
            {
                var roles = await _userManager.GetRolesAsync(u);
                string role = roles.Contains("Teacher") ? "Teacher" : (roles.Contains("Student") ? "Student" : "Other");
                classmates.Add((
                    new UserDto
                    {
                        Id = u.Id,
                        UserName = u.UserName ?? string.Empty,
                        Email = u.Email ?? string.Empty,
                        CourseId = u.CourseId,
                        Roles = roles.ToList()
                    },
                    role
                ));
            }

            // Sort: teachers first, then students, then others, then by name
            var sorted = classmates
                .OrderByDescending(c => c.role == "Teacher")
                .ThenByDescending(c => c.role == "Student")
                .ThenBy(c => c.user.UserName)
                .Select(c => new {
                    c.user.Id,
                    c.user.UserName,
                    c.user.Email,
                    c.user.CourseId,
                    role = c.role
                })
                .ToList();

            return Ok(sorted);
        }
    }
}
