namespace Domain.Contracts.Repositories;

public interface IUnitOfWork
{
    ICourseRepository Courses { get; }
    IModuleRepository Modules { get; }
    IActivityRepository Activities { get; }
    IActivityTypeRepository ActivityTypes { get; }
    Task CompleteAsync();
}