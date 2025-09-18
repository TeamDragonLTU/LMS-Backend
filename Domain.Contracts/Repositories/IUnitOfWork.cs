namespace Domain.Contracts.Repositories;

public interface IUnitOfWork
{
    ICourseRepository Courses { get; }
    IModuleRepository ModuleRepository { get; }
    Task CompleteAsync();
}