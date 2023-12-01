using ERDiagrams.Back.Models;
using ERDiagrams.Back.Repositories.Interfaces;

namespace ERDiagrams.Back.Repositories;

public class BookRepository : Repository<Book>
{
    public BookRepository(CosmosContext dbContext) : base(dbContext){}
}