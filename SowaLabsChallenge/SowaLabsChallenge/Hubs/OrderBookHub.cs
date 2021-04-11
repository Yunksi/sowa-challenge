using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SowaLabsChallenge.Hubs
{
    public class OrderBookHub: Hub<IOrderBookHub>
    {
        private readonly ILogger<OrderBookHub> _logger;

        public OrderBookHub(ILogger<OrderBookHub> logger)
        {
            _logger = logger;
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"SignalR user has connected with connection Id: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }
    }
}