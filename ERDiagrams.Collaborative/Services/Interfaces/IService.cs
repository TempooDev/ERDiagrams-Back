using System.Linq.Expressions;


namespace ERDiagrams.Collaborative.Services.Interfaces;

public interface IService<T> where T : class
{
    public Task<T> GetById(string id);
    public Task<IEnumerable<T>> GetAll();
    public Task Create(T entity);
    public Task Update(string id,T entity);
    public Task Delete(string id);   
}