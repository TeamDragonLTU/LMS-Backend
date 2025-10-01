using System.Linq.Expressions;

namespace Domain.Contracts.Repositories;
public interface IRepositoryBase<T>
{
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);

}
