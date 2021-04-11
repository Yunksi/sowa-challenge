using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SowaLabsChallenge
{
    public class Worker: BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Fetch data from BINANCE API
                //Calculate market depth
                //Notify clients connected to SignalR hub
                //Save returned data to file system
                await Task.Delay(10000);
            }
        }
    }
}