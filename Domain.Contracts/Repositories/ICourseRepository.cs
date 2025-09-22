using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    public interface ICourseRepository : IRepositoryBase<Course>
    {
        Task<IEnumerable<Course>> GetCoursesAsync();
        Task<Course?> GetCourseAsync(Guid id);
        Task<bool> AnyCourseAsync(Guid id);
        Task<int> CourseCountAsync();

    }
}
