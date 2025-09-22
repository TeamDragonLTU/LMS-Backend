using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{
    private Lazy<IAuthService> authService;
    private Lazy<ICourseService> courseService;
    private Lazy<IModuleService> moduleService;
    private Lazy<IActivityService> activityService;
    private Lazy<IActivityTypeService> activityTypeService;
    
    public IAuthService AuthService => authService.Value;
    public ICourseService CourseService => courseService.Value;
    public IModuleService ModuleService => moduleService.Value;
    public IActivityService ActivityService=> activityService.Value;
    public IActivityTypeService ActivityTypeService => activityTypeService.Value;

    public ServiceManager(Lazy<IAuthService> authService, Lazy<ICourseService> courseService, Lazy<IModuleService> moduleService, Lazy<IActivityService> activityService, Lazy<IActivityTypeService> activityTypeService)
    {
        this.authService = authService;
        this.courseService = courseService;
        this.moduleService = moduleService;
        this.activityService = activityService;
        this.activityTypeService = activityTypeService;
    }
}
