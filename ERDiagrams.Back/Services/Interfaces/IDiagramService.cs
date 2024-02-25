using System.Threading.Tasks;
using ERDiagrams.Back.Models;

namespace ERDiagrams.Back.Services.Interfaces;

public interface IDiagramService:IService<Diagram>
{
    public Task<bool> CheckForConflictingDiagram(Diagram diagram);
}