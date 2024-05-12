using System.Linq.Expressions;
using ERDiagrams.Collaborative.Models.Interfaces;

namespace ERDiagrams.Collaborative.Repositories.Interfaces;

public interface IRepository<T> where T: class, IEntity
{
    public Task<T> GetAsync(string id);
    public Task<IEnumerable<T>> GetAsync();
    public Task CreateAsync(T entity);
    public Task UpdateAsync(string id,T entity);
    public Task RemoveAsync(string id);

}