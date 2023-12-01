using System.Linq;
using System.Threading.Tasks;
using ERDiagrams.Back.Models;
using ERDiagrams.Back.Repositories.Interfaces;
using ERDiagrams.Back.Services.Interfaces;

namespace ERDiagrams.Back.Services;

public class BookService :Service<Book>,IBookService
{
    public BookService(IRepository<Book> repository) : base(repository)
    {
    }

    public async Task<bool> CheckForConflictingBook(Book book)
    {
        return (await _repository.GetByCondition(x => x.Title == book.Title)).Any();
    }
}