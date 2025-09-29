using LMS.Shared.DTOs.ApplicationUserDtos;
using LMS.Shared.DTOs.CourseDtos;

namespace Service.Contracts
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto> GetCourseAsync(Guid id);
        Task PutCourseAsync(Guid id, UpdateCourseDto dto);
        Task<CourseDto> PostCourseAsync(CreateCourseDto dto);
        Task DeleteCourseAsync(Guid id);
        Task<CourseDetailsDto> GetCourseWithModulesAndActivitiesAsync(string userId);
        Task<IEnumerable<ApplicationUserDto>> GetCourseParticipantsByUserIdAsync(string userId);
    }
}
