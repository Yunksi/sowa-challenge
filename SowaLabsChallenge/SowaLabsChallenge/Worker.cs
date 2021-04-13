using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SowaLabsChallenge.Hubs;
using SowaLabsChallenge.Services.Calculation;
using SowaLabsChallenge.Services.FetchData;

namespace SowaLabsChallenge
{
    public class Worker: BackgroundService
    {
        private readonly IFetchDataService _fetchDataService;
        private readonly ICalculationService _calculationService;
        private readonly IHubContext<OrderBookHub, IOrderBookHub> _orderBookHub;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public Worker(
            IFetchDataService fetchDataService, 
            ICalculationService calculationService,
            IHubContext<OrderBookHub, IOrderBookHub> orderBookHub,
            IWebHostEnvironment env,
            ILogger<Worker> logger)
        {
            _fetchDataService = fetchDataService;
            _calculationService = calculationService;
            _orderBookHub = orderBookHub;
            _env = env;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Fetch data from BINANCE API for BTC/EUR pair
                var orderBookBtcEur =
                    await _fetchDataService.FetchOrderBookDataFromUrl("https://api.binance.com/api/v3/depth?symbol=BTCEUR&limit=1000");
                // Fetch data from BINANCE API for BTC/USDC pair
                var orderBookBtcUsd =
                    await _fetchDataService.FetchOrderBookDataFromUrl("https://api.binance.com/api/v3/depth?symbol=BTCUSDC&limit=1000");
                // Calculate market depth
                // Calculate order book depth for BTCEUR
                var calculatedOrderBookDepthBtcEur = await _calculationService.CalculateOrderBookDepth(orderBookBtcEur);
                // Calculate order book market depth for BTCUSDC
                var calculatedOrderBookDepthBtcUsd = await _calculationService.CalculateOrderBookDepth(orderBookBtcUsd);
                var serializedBtcEurOrderBookDepthDto = JsonSerializer.Serialize(calculatedOrderBookDepthBtcEur);
                var serializedBtUsdOrderBookDepthDto = JsonSerializer.Serialize(calculatedOrderBookDepthBtcUsd);
                // Notify clients connected to SignalR hub
                // Notify all clients subscribed to BTCEUR
                await _orderBookHub.Clients.Group("BTCEUR").UpdateOrderBook(serializedBtcEurOrderBookDepthDto);
                // Notify all clients subscribed to BTCUSDC
                await _orderBookHub.Clients.Group("BTCUSD").UpdateOrderBook(serializedBtUsdOrderBookDepthDto);
                // Save returned data to file system
                // For the sake of simplicity and purpose of the challenge we will write audit log to file system
                // In the real case we would write audit to the database
                using (var sw = File.CreateText($"{_env.ContentRootPath}/Audit/{DateTimeOffset.Now.ToUnixTimeSeconds()}_BTCEUR.txt"))
                {
                    await sw.WriteAsync(serializedBtcEurOrderBookDepthDto);
                }
                using (var sw = File.CreateText($"{_env.ContentRootPath}/Audit/{DateTimeOffset.Now.ToUnixTimeSeconds()}_BTCUSD.txt"))
                {
                    await sw.WriteAsync(serializedBtUsdOrderBookDepthDto);
                }
                await Task.Delay(1000);
            }
        }
    }
}