using ERDiagrams.Collaborative.Hubs;
using ERDiagrams.Collaborative.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ERDiagrams.Collaborative.Controllers;
[Route("api/[controller]")]
public class BoardController:ControllerBase
{
    private IHubContext<BoardHub> _boardContext;
    private readonly IDictionary<string, string> _connections;
    public BoardController(IHubContext<BoardHub> boardContext, IDictionary<string,string> connections)
    {
        _boardContext = boardContext;
        _connections = connections;
    }
  
}