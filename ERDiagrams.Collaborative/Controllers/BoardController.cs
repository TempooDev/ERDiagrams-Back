using ERDiagrams.Collaborative.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ERDiagrams.Collaborative.Controllers;
[Route("api/[controller]")]
public class BoardController:ControllerBase
{
    private IHubContext<BoardHub> _boardContext;

    public BoardController(IHubContext<BoardHub> boardContext)
    {
        _boardContext = boardContext;
    }
    [HttpGet]
    public async Task<IActionResult> Send(string message)
    {
        await _boardContext.Clients.All.SendAsync("sendMessage", message);
        return Ok();
    }
}