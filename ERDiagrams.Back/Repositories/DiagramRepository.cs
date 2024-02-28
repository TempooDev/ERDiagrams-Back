using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ERDiagrams.Back.Models;
using ERDiagrams.Back.Models.Interfaces;
using ERDiagrams.Back.Repositories.Interfaces;

namespace ERDiagrams.Back.Repositories;

public class DiagramRepository : Repository<Diagram>
{
    public DiagramRepository(CosmosContext dbContext) : base(dbContext){}

   
}