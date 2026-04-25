using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApplication1.Hubs
{
    [Authorize] 
    public class AppHub : Hub
    {
      
        public async Task SendMessage(string message)
        {
            var userName = Context.User?.Identity?.Name ?? "Аноним";
            await Clients.All.SendAsync("ReceiveMessage", userName, message, System.DateTime.Now);
        }

       
        public async Task NotifyDataChanged(string entityName, string action)
        {
            await Clients.All.SendAsync("DataChanged", entityName, action);
        }
    }
}