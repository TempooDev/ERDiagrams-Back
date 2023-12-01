using System.Threading.Tasks;
using ERDiagrams.Back.Models;

namespace ERDiagrams.Back.Services.Interfaces;

public interface IBookService :IService<Book>
{
    public Task<bool> CheckForConflictingBook(Book book);
}