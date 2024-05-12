using ERDiagrams.Collaborative.Models;
using ERDiagrams.Collaborative.Repositories.Interfaces;
using ERDiagrams.Collaborative.Services.Interfaces;

namespace ERDiagrams.Collaborative.Services;

public class DiagramService: Service<Diagram>,IDiagramService
{
    private IRepository<Diagram> _repository;

    public DiagramService(IRepository<Diagram> repository) : base(repository)
    {
        _repository = repository;
    }
    public async Task<bool> CheckForConflictingDiagram(Diagram diagram)
    {
        return (await _repository.GetByCondition(x => x.Id == diagram.Id)).Any();
    }
}

