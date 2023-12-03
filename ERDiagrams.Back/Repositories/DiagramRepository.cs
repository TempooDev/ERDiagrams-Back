using ERDiagrams.Back.Models;
using ERDiagrams.Back.Repositories.Interfaces;

namespace ERDiagrams.Back.Repositories;

public class DiagramRepository : Repository<Diagram>
{
    public DiagramRepository(CosmosContext dbContext) : base(dbContext){}
    
}