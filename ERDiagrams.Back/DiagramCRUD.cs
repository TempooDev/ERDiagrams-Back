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

public  class DiagramCrud
{
    private readonly IDiagramService _diagramService;

    public DiagramCrud(IDiagramService diagramService)
    {
        _diagramService = diagramService;
    }

    #region CRUD

    [FunctionName("GetDiagramById")]
    public async Task<IActionResult> GetDiagramById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "diagrams/{id}")] HttpRequest req, ILogger log,string id)
    {
        try
        {
            var diagram = await _diagramService.GetById(id);
            if (diagram is null)
            {
                log.LogWarning($"No diagram exits with id: {id}");
                return new UnprocessableEntityObjectResult($"No diagram exits with id: {id}");
            }

            return new OkObjectResult(diagram);
        }
        catch (Exception e)
        {
            
            var errorMessage = $"Failed to fetch a diagram with id: {id}";

            log.LogError(e, errorMessage);
            return new InternalServerErrorResult();
        }
    }
    
    [FunctionName("GetDiagrams")]
    public async Task<IActionResult> GetAllDiagrams([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "diagrams")]
        HttpRequest req, ILogger log)
    {
        try
        {
            var diagrams = await _diagramService.GetAll();
            if (diagrams is null)
            {
                log.LogWarning($"Diagrams not found");
                return new UnprocessableEntityObjectResult($"Diagrams not found");
            }
            return new OkObjectResult(diagrams);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to fetch a diagrams.";

            log.LogError(e, errorMessage);
            return new InternalServerErrorResult();
        }
    }
    
    [FunctionName("GetDiagramByUserId")]
    public async Task<IActionResult> GetDiagramByUserId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "diagrams/{userid}")] HttpRequest req, ILogger log,string userId)
    {
        try
        {
            var diagram = await _diagramService.GetByCondition(x=>x.User==userId);
            if (diagram is null)
            {
                log.LogWarning($"No diagram exits with id: {userId}");
                return new UnprocessableEntityObjectResult($"No diagram exits with id: {userId}");
            }

            return new OkObjectResult(diagram);
        }
        catch (Exception e)
        {
            
            var errorMessage = $"Failed to fetch a diagram with id: {userId}";

            log.LogError(e, errorMessage);
            return new InternalServerErrorResult();
        }
    }
    [FunctionName("CreateDiagram")]
    public  async Task<IActionResult> CreateDiagrams(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "diagrams")] HttpRequest req, ILogger log)
    {
        var diagramJson = await req.ReadAsStringAsync();
        try
        {
            var diagram = JsonConvert.DeserializeObject<Diagram>(diagramJson);
            diagram.Id = Guid.NewGuid().ToString();
            if (await _diagramService.CheckForConflictingDiagram(diagram))
            {
                log.LogWarning($"Diagram with matching title already exists in library: \"{diagram.Id}\"");
                return new ConflictObjectResult(
                    $"Diagram with matching title already exists in library: \"{diagram.Id}\"");
            }

            await _diagramService.Create(diagram);

            return new OkObjectResult(diagram);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to create a diagram: {diagramJson}";
            
            log.LogError(e,errorMessage);
            return new BadRequestObjectResult(errorMessage);
        }
    }

    [FunctionName("DeleteDiagram")]
    public async Task<IActionResult> DeleteDiagram(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "diagrams/{id}")] HttpRequest req, ILogger log,
        string id)
    {
        try
        {
            await _diagramService.Delete(id);

            return new NoContentResult();
        }
        catch (Exception e)
        {
            var error = $"Failed to delete diagram with id:{id}";
            log.LogError(e,error);
            return new InternalServerErrorResult();
        }
        
    }

    [FunctionName("UpdateDiagram")]
    public async Task<IActionResult> UpdateDiagram(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "diagram/{id}")] HttpRequest req, ILogger log,
        string id)
    {
        var diagramJson = await req.ReadAsStringAsync();

        try
        {
            var diagram = JsonConvert.DeserializeObject<Diagram>(diagramJson);
            diagram.Id = id;

            await _diagramService.Update(diagram);
            return new OkObjectResult(diagram);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to update diagram with id: {id} with details: {diagramJson}";

            log.LogError(e, errorMessage);
            return new BadRequestObjectResult(errorMessage);
        }

    }
    #endregion
    

}