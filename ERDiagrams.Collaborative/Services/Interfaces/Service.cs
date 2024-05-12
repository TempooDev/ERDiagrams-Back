using System.Linq.Expressions;

using ERDiagrams.Collaborative.Repositories.Interfaces;

namespace ERDiagrams.Collaborative.Services.Interfaces;

public abstract class Service<T> :IService<T> where T:class
{
    protected readonly IRepository<T> _repository;

    protected Service(IRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<T> GetById(string id) => await _repository.GetAsync(id);

    public virtual async Task<IEnumerable<T>> GetAll() => await _repository.GetAsync();


    public virtual async Task Create(T entity) => await _repository.CreateAsync(entity);

    public virtual async Task Update(string id,T entity) => await _repository.UpdateAsync(id, entity);

    public virtual async Task Delete(string id) => await _repository.RemoveAsync(id);

}