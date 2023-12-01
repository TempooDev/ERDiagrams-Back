using System;
using System.Threading.Tasks;
using ERDiagrams.Back.Models;
using ERDiagrams.Back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ERDiagrams.Back;

public  class BookCrud
{
    private readonly IBookService _bookService;

    public BookCrud(IBookService bookService)
    {
        _bookService = bookService;
    }
    [FunctionName("CreateBook")]
    public  async Task<IActionResult> CreateBook(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "books")] HttpRequest req, ILogger log)
    {
        var bookJson = await req.ReadAsStringAsync();
        try
        {
            var book = JsonConvert.DeserializeObject<Book>(bookJson);
            if (await _bookService.CheckForConflictingBook(book))
            {
                return new ConflictObjectResult(
                    $"Book with matching title already exists in library: \"{book.Title}\"");
            }

            await _bookService.Create(book);

            return new OkObjectResult(book);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to create a book: {bookJson}";
            
            log.LogError(e,errorMessage);
            return new BadRequestObjectResult(errorMessage);
        }

        
    }
}