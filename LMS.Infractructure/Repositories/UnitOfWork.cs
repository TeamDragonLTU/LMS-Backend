using Domain.Contracts.Repositories;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;
    private readonly Lazy<ICourseRepository> _courseRepository;
    public ICourseRepository Courses => _courseRepository.Value;

    public UnitOfWork(ApplicationDbContext context)
    {
        _courseRepository = new Lazy<ICourseRepository>(() => new CourseRepository(context));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
