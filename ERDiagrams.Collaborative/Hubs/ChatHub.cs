using Microsoft.AspNetCore.SignalR;

namespace ERDiagrams.Collaborative.Hubs;
public class UserMessage
{
    public required string SenderId { get; set; }
    public required string SenderUser { get; set; }

    public required string Content { get; set; }
}
public class ChatHub:Hub
{
   
  
        private static readonly List<UserMessage> MessageHistory = new List<UserMessage>();
        public async Task PostMessage(UserMessage userMessage)
        {
            MessageHistory.Add(userMessage);
            
            await Clients.Others.SendAsync("ReceiveMessage", userMessage);
        }
        public async Task RetrieveMessageHistory() => 
            await Clients.Caller.SendAsync("MessageHistory", MessageHistory);
    
}