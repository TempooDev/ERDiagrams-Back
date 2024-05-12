using System.Linq.Expressions;

namespace ERDiagrams.Collaborative.Repositories.Interfaces;

public interface IRepository<T> where T: class
{
    public Task<T> GetAsync(string id);
    public Task<List<T>> GetAsync();
    public Task CreateAsync(T entity);
    public Task UpdateAsync(string id,T entity);
    public Task RemoveAsync(string id);
    
    public Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> predicate);

}