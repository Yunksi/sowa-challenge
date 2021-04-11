using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SowaLabsChallenge.Services.FetchData;

namespace SowaLabsChallenge
{
    public class Worker: BackgroundService
    {
        private readonly IFetchDataService _fetchDataService;

        public Worker(IFetchDataService fetchDataService)
        {
            _fetchDataService = fetchDataService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Fetch data from BINANCE API for BTC/EUR pair
                var orderBookBtcEur =
                    await _fetchDataService.FetchOrderBookDataFromUrl("https://api.binance.com/api/v3/depth?symbol=BTCEUR&limit=1000");
                //Fetch data from BINANCE API for BTC/USDC pair
                var orderBookBtcUsd =
                    await _fetchDataService.FetchOrderBookDataFromUrl("https://api.binance.com/api/v3/depth?symbol=BTCUSDC&limit=1000");
                //Calculate market depth
                //Notify clients connected to SignalR hub
                //Save returned data to file system
                await Task.Delay(10000);
            }
        }
    }
}