using Microsoft.AspNetCore.SignalR;
using web_petshelter.Models;

namespace web_petshelter.RealTime
{
    public interface ITasksClient
    {
        Task TaskAdded(VolunteerTask task);
        Task TaskUpdated(VolunteerTask task);
        Task TaskDeleted(int id);
        Task Presence(string user, bool online);
    }

    public class TasksHub : Hub<ITasksClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.Presence(Context.User?.Identity?.Name ?? "guest", true);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            await Clients.All.Presence(Context.User?.Identity?.Name ?? "guest", false);
            await base.OnDisconnectedAsync(ex);
        }
    }
}
