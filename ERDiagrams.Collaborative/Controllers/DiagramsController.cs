using ERDiagrams.Collaborative.Models;
using ERDiagrams.Collaborative.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ERDiagrams.Collaborative.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DiagramsController:ControllerBase
{
     private readonly IDiagramService _diagramService;
  
    public DiagramsController(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        //TODO: ADD LOGGER
    }
    
    
    
    #region CRUD
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetDiagramById(
        string id)
    {
        // var authenticationResult = await _apiAuthentication.AuthenticateAsync(req.Headers);
        // if (authenticationResult.Failed)
        // {
        //     log.LogWarning(authenticationResult.FailureReason);
        //     return new UnauthorizedResult();
        // }
        // log.LogInformation(authenticationResult.User.Identity.Name);
        //
        try
        {
            var diagram = await _diagramService.GetById(id);
            if (diagram is null)
            {
               
                return new UnprocessableEntityObjectResult($"No diagram exits with id: {id}");
            }
    
            return new OkObjectResult(diagram);
        }
        catch (Exception e)
        {
            
            var errorMessage = $"Failed to fetch a diagram with id: {id}";
    
            return new BadRequestResult();
        }
    }
    
[HttpGet]
public async Task<IActionResult> GetAllDiagrams(
      )
    {
        try
        {
            var diagrams = await _diagramService.GetAll();
            if (diagrams is null)
            {
                
                return new UnprocessableEntityObjectResult($"Diagrams not found");
            }
            return new OkObjectResult(diagrams);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to fetch a diagrams.";
    
         
            return new BadRequestResult();
        }
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetDiagramByUserId(
        string userId)
    {
        try
        {
            var diagram = await _diagramService.GetByCondition(x=> x.UserId == userId);
            if (diagram is null)
            {
                
                return new UnprocessableEntityObjectResult($"No diagram exits with id: {userId}");
            }
    
            return new OkObjectResult(diagram);
        }
        catch (Exception e)
        {
            
            var errorMessage = $"Failed to fetch a diagram with id: {userId}";
    
            
            return new BadRequestResult();
        }
    }
    [HttpPost]
    public  async Task<IActionResult> CreateDiagrams(
       Diagram diagram  )
    {
     
        
        try
        {
           
            diagram._id = Guid.NewGuid().ToString();
            if (await _diagramService.CheckForConflictingDiagram(diagram))
            {
                
                return new ConflictObjectResult(
                    $"Diagram with matching title already exists in library: \"{diagram._id}\"");
            }
    
            await _diagramService.Create(diagram);
    
            return new OkObjectResult(diagram);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to create a diagram: {diagram}";
            
            
            return new BadRequestObjectResult(errorMessage);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiagram(
      
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
          
            return new BadRequestResult();
        }
        
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDiagram(
        
        string id, Diagram diagram)
    {
       
    
        try
        {
           
            diagram._id = id;
    
            await _diagramService.Update(id,diagram);
            return new OkObjectResult(diagram);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to update diagram with id: {id} with details: {diagram}";
    
            
            return new BadRequestObjectResult(errorMessage);
        }
    
    }
    #endregion
}