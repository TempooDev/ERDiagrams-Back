using ERDiagrams.Collaborative.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR;

namespace ERDiagrams.Collaborative.Hubs;

public class BoardHub : Hub
{
    private readonly IDictionary<string, UserConnectionRoom> _connections;
    public BoardHub(IDictionary<string, UserConnectionRoom> connections)
    {
        _connections = connections;
    }
   
    public async Task JoinGroup(UserConnectionRoom userConnection)
    {
        await Groups.AddToGroupAsync(this.Context.ConnectionId, userConnection.Room);
        _connections[Context.ConnectionId]=userConnection;
        await Clients.Group(userConnection.Room).SendAsync("receiveMessage", $"{userConnection.User} has joined the group .");
        await SendConnectedUser(userConnection.Room);
    }
    public async Task Send(Diagram diagram)
    {
        if(_connections.TryGetValue(this.Context.ConnectionId, out UserConnectionRoom user))
        {
            await Clients.Group(user.Room).SendAsync("sendDiagram", diagram, user.User);
        }
    
    }
    public  Task SendConnectedUser(string room)
    {
        var users = _connections.Values.Where(x =>x.Room==room).Select(s=>s.User);
        return Clients.Group(room).SendAsync("receiveConnectedUser", users);
    }
   
    public override Task OnDisconnectedAsync(Exception? exp)
    {
        if(!_connections.TryGetValue(Context.ConnectionId,out UserConnectionRoom userConnectionRoom))
        {
            return base.OnDisconnectedAsync(exp);
        }
        Clients.Group(userConnectionRoom.Room!).SendAsync("receiveMessage", $"{userConnectionRoom.User} has left the group.");
        SendConnectedUser(userConnectionRoom.Room!);
        return base.OnDisconnectedAsync(exp);
    }
}