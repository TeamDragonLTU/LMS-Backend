using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    public interface ICourseRepository : IRepositoryBase<Course>
    {
        Task<IEnumerable<Course>> GetCoursesAsync();
        Task<Course?> GetCourseAsync(Guid id, bool trackChanges = false);
        Task<bool> AnyCourseAsync(Guid id);
        Task<int> CourseCountAsync();
        Task<Course?> GetCourseWithModulesAndActivitiesAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetCourseParticipantsByUserIdAsync(string userId);

    }
}
