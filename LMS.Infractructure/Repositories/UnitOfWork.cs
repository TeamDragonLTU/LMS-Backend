using Domain.Contracts.Repositories;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;
    private readonly Lazy<ICourseRepository> _courseRepository;
    private readonly Lazy<IActivityRepository> _activityRepository;
    private readonly Lazy<IActivityTypeRepository> _activityTypeRepository;
    private readonly Lazy<IModuleRepository> _moduleRepository;
    public ICourseRepository Courses => _courseRepository.Value;
    public IActivityRepository Activities => _activityRepository.Value;
    public IActivityTypeRepository ActivityTypes => _activityTypeRepository.Value;
    public IModuleRepository Modules => _moduleRepository.Value;




    public UnitOfWork(ApplicationDbContext context)
    {
        _courseRepository = new Lazy<ICourseRepository>(() => new CourseRepository(context));
        _activityRepository = new Lazy<IActivityRepository>(() => new ActivityRepository(context));
        _activityTypeRepository = new Lazy<IActivityTypeRepository>(() => new ActivityTypeRepository(context));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        _moduleRepository = new Lazy<IModuleRepository>(() => new ModuleRepository(context));

    }

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
