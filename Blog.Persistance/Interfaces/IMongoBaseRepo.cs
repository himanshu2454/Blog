

using System.Linq.Expressions;

namespace Blog.Persistance.Interfaces;

public interface IMongoBaseRepo<T>
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(Expression<Func<T, bool>> filter);
    Task CreateAsync(T entity);
    Task UpdateAsync(Expression<Func<T, bool>> filter, T entity);
    Task DeleteAsync(Expression<Func<T, bool>> filter);
}
