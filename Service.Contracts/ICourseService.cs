using LMS.Shared.DTOs.CourseDtos;

namespace Service.Contracts
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto> GetMovieAsync(Guid id);
    }
}
