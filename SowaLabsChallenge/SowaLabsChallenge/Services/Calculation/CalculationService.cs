using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SowaLabsChallenge.Models;

namespace SowaLabsChallenge.Services.Calculation
{
    public class CalculationService: ICalculationService
    {
        public async Task<OrderBookDepthDto> CalculateOrderBookDepth(OrderBook orderBook)
        {
            var orderBookDepthDto = new OrderBookDepthDto
            {
                Top10Bids = new List<List<decimal>>(), 
                Top10Asks = new List<List<decimal>>(),
                Bids = new List<List<decimal>>(), 
                Asks = new List<List<decimal>>()
            };
            
            // First we will calculate market depth for the bids
            // We assume that orderBook contains already sorted data
            // For bids from High->Low, for Asks Low-High
            // If this condition isn't met, we should sort the data first.
            decimal volume = 0;
            var counter = 0;
            foreach (var priceQuantityList in orderBook.Bids)
            {
                var volumeForThePrice = Convert.ToDecimal(priceQuantityList[1]);;
                var price = Convert.ToDecimal(priceQuantityList[0]);
                volume += volumeForThePrice;
                var list = new List<decimal> {price, volume};
                orderBookDepthDto.Bids.Add(list);
                if (counter > 10) continue;
                orderBookDepthDto.Top10Bids.Add(new List<decimal>{price, volumeForThePrice});
                counter++;
            }
            
            //Calculation of the market depth for asks
            volume = 0;
            counter = 0;
            foreach (var priceQuantityList in orderBook.Asks)    
            {
                var volumeForThePrice = Convert.ToDecimal(priceQuantityList[1]);;
                var price = Convert.ToDecimal(priceQuantityList[0]);
                volume += volumeForThePrice;
                var list = new List<decimal> {price, volume};
                orderBookDepthDto.Asks.Add(list);
                if (counter > 10) continue;
                orderBookDepthDto.Top10Asks.Add(new List<decimal>{price, volumeForThePrice});
                counter++;

            }
            
            return orderBookDepthDto;
        }
    }
}