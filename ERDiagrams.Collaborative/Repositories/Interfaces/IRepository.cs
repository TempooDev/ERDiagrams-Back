using System.Linq.Expressions;
using ERDiagrams.Collaborative.Models.Interfaces;

namespace ERDiagrams.Collaborative.Repositories.Interfaces;

public interface IRepository<T> where T: class, IEntity
{
    public Task<T> GetById(string id);
    public Task<IEnumerable<T>> GetAll();
    public Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression);
    public Task<T> Create(T entity);
    public Task<T> Update(T entity);
    public Task Delete(string id);

}