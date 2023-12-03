using System;
using System.Threading.Tasks;
using System.Web.Http;
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

    #region CRUD

     [FunctionName("GetBookById")]
    public async Task<IActionResult> GetBookById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books/{id}")] HttpRequest req, ILogger log,string id)
    {
        try
        {
            var book = await _bookService.GetById(id);
            if (book is null) return new UnprocessableEntityObjectResult($"No book exits with id: {id}");
            return new OkObjectResult(book);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to fetch a book with id: {id}";

            log.LogError(e, errorMessage);
            return new InternalServerErrorResult();
        }
      
    }

    [FunctionName("GetBooks")]
    public async Task<IActionResult> GetAllBooks([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books")]
        HttpRequest req, ILogger log)
    {
        try
        {
            var books = await _bookService.GetAll();
            if (books is null) return new UnprocessableEntityObjectResult($"Book not found");
            return new OkObjectResult(books);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to fetch a books.";

            log.LogError(e, errorMessage);
            return new InternalServerErrorResult();
        }
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

    [FunctionName("DeleteBook")]
    public async Task<IActionResult> DeleteBook(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "books/{id}")] HttpRequest req, ILogger log,
        string id)
    {
        try
        {
            await _bookService.Delete(id);

            return new NoContentResult();
        }
        catch (Exception e)
        {
            var error = $"Failed to delete book with id:{id}";
            log.LogError(e,error);
            return new InternalServerErrorResult();
        }
        
    }

    [FunctionName("UpdateBook")]
    public async Task<IActionResult> UpdateBook(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "book/{id}")] HttpRequest req, ILogger log,
        string id)
    {
        var bookJson = await req.ReadAsStringAsync();

        try
        {
            var book = JsonConvert.DeserializeObject<Book>(bookJson);
            book.Id = id;

            await _bookService.Update(book);
            return new OkObjectResult(book);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to update book with id: {id} with details: {bookJson}";

            log.LogError(e, errorMessage);
            return new BadRequestObjectResult(errorMessage);
        }

    }
    #endregion 
}