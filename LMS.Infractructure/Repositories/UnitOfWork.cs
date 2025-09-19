using Domain.Contracts.Repositories;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;
    private readonly Lazy<ICourseRepository> _courseRepository;
    private readonly Lazy<IActivityRepository> _activityRepository;
    private readonly Lazy<IActivityTypeRepository> _activityTypeRepository;
    public ICourseRepository Courses => _courseRepository.Value;
    public IActivityRepository Activities => _activityRepository.Value;
    public IActivityTypeRepository ActivityTypes => _activityTypeRepository.Value;



    public UnitOfWork(ApplicationDbContext context)
    {
        _courseRepository = new Lazy<ICourseRepository>(() => new CourseRepository(context));
        _activityRepository = new Lazy<IActivityRepository>(() => new ActivityRepository(context));
        _activityTypeRepository = new Lazy<IActivityTypeRepository>(() => new ActivityTypeRepository(context));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
