using Microsoft.AspNetCore.SignalR;

namespace ERDiagrams.Collaborative.Hubs;
public class Message
{
    public string User { get; set; }
    public string Content { get; set; }
}
public class ChatHub: Hub
{
    private static readonly List<Message> MessageHistory = new List<Message>();
    public async Task SendMessage(string user, string message)
    {
        var messageObj = new Message { User = user, Content = message };
        MessageHistory.Add(messageObj);
        await Clients.All.SendAsync("ReceiveMessage", user, message);

    }
    
    public async Task RetrieveMessageHistory()
    {
        await Clients.Caller.SendAsync("MessageHistory", MessageHistory);
    }
}
