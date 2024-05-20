using System.Linq.Expressions;
using ERDiagrams.Collaborative.Models;

namespace ERDiagrams.Collaborative.Services.Interfaces;

public interface IDiagramService:IService<Diagram>
{
    public Task<bool> CheckForConflictingDiagram(Diagram diagram);
    public Task<IEnumerable<Diagram>?> GetByCondition(Expression<Func<Diagram,bool>>func);
}