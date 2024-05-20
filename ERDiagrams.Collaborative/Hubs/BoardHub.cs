using ERDiagrams.Collaborative.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ERDiagrams.Collaborative.Hubs;

public class BoardHub : Hub
{
   
  
    public async Task Send(Diagram diagram)
    {
       
            await Clients.All.SendAsync("sendDiagram", diagram);
      
    
    }
  
   
  
}