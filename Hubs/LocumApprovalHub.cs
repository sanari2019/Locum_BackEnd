using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Locum_Backend.Hubs
{
    public class LocumApprovalHub : Hub
    {
        // Add hub methods for real-time communication here
        // For instance:
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task UpdateApprovalStatus(int trackingId, string status)
        {
            // Update approval status logic here

            // Broadcast the updated status to clients subscribed to this package
            await Clients.Group($"package-{trackingId}").SendAsync("ApprovalStatusUpdated", trackingId, status);
        }

        public async Task JoinPackageGroup(int packageId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"package-{packageId}");
        }

        public async Task LeavePackageGroup(int packageId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"package-{packageId}");
        }
    }
}
