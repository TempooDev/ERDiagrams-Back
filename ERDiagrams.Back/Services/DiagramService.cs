using System.Linq;
using System.Threading.Tasks;
using ERDiagrams.Back.Models;
using ERDiagrams.Back.Repositories.Interfaces;
using ERDiagrams.Back.Services.Interfaces;

namespace ERDiagrams.Back.Services;

public class DiagramService: Service<Diagram>,IDiagramService
{
    public DiagramService(IRepository<Diagram> repository) : base(repository)
    {
    }

    public async Task<bool> CheckForConflictingDiagram(Diagram diagram)
    {
        return (await _repository.GetByCondition(x => x.Id == diagram.Id)).Any();
    }
}