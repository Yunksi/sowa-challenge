using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SowaLabsChallenge.Services.Calculation;
using SowaLabsChallenge.Services.FetchData;

namespace SowaLabsChallenge
{
    public class Worker: BackgroundService
    {
        private readonly IFetchDataService _fetchDataService;
        private readonly ICalculationService _calculationService;

        public Worker(
            IFetchDataService fetchDataService, 
            ICalculationService calculationService)
        {
            _fetchDataService = fetchDataService;
            _calculationService = calculationService;
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
                //Calculate order book depth for BTCEUR
                var calculatedOrderBookDepthBtcEur = await _calculationService.CalculateOrderBookDepth(orderBookBtcEur);
                // Calculate order book market depth for BTCUSDC
                var calculatedOrderBookDepthBtcUsd = await _calculationService.CalculateOrderBookDepth(orderBookBtcUsd);0
                //Notify clients connected to SignalR hub
                //Save returned data to file system
                await Task.Delay(10000);
            }
        }
    }
}