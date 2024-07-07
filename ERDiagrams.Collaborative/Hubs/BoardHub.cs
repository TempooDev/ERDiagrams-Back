using ERDiagrams.Collaborative.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ERDiagrams.Collaborative.Hubs;

public class BoardHub : Hub
{
    public async Task AddToGroup(string diagramId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, diagramId);

        await Clients.Group(diagramId).SendAsync("Send", $"{Context.ConnectionId} has joined the group of diagramId: {diagramId}.");
    }

    public async Task RemoveFromGroup(string diagramId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, diagramId);

        await Clients.Group(diagramId).SendAsync("Send", $"{Context.ConnectionId} has left the group of diagramId: {diagramId}.");
    }
    
    public async Task SendDiagram(Diagram diagram)
    {

        await Clients.Group(diagram.diagramId).SendAsync("sendDiagram", diagram);

    }
  
   
  
}