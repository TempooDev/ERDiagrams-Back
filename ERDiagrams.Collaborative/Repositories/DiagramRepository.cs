using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ERDiagrams.Collaborative.Models;
using ERDiagrams.Collaborative.Models.Interfaces;
using ERDiagrams.Collaborative.Repositories.Interfaces;
using ERDiagrams.Collaborative.Models;

namespace ERDiagrams.Collaborative.Repositories;

public class DiagramRepository : Repository<Diagram>
{
    public DiagramRepository(CosmosContext dbContext) : base(dbContext){}

   
}