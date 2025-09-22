using Domain.Contracts.Repositories;
using LMS.Infractructure.Data;
using LMS.Infrastructure.Repositories;

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




    public UnitOfWork(Lazy<ICourseRepository> courseRepository, Lazy<IModuleRepository> moduleRepository, Lazy<IActivityRepository> activityRepository, Lazy<IActivityTypeRepository> activityTypeRepository)
    {
        _courseRepository = courseRepository;
        _moduleRepository = moduleRepository;
        _activityRepository = activityRepository;
        _activityTypeRepository = activityTypeRepository;
        this.context = context ?? throw new ArgumentNullException(nameof(context));

    }

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
