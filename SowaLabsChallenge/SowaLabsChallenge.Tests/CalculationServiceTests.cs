using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using SowaLabsChallenge.Services.Calculation;
using Xunit;

namespace SowaLabsChallenge.Tests
{
    public class CalculationServiceTests
    {
        private readonly ICalculationService _calculationService;

        public CalculationServiceTests(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }
        
        [Fact]
        public async Task CalculationService_ShouldCalculateMarketDepth()
        {
            var orderBook = new Models.OrderBook
            {
                Bids = new List<List<string>>
                {
                    new()
                    {
                        "100",
                        "10"
                    },
                    new()
                    {
                        "99",
                        "9"
                    },
                    new()
                    {
                        "98",
                        "8"
                    }
                },
                Asks = new List<List<string>>
                {
                    new()
                    {
                        "101",
                        "1"
                    },
                    new()
                    {
                        "102",
                        "2"
                    },
                    new()
                    {
                        "103",
                        "3"
                    }
                }
            };

            var calculatedOrderBookDepth = await _calculationService.CalculateOrderBookDepth(orderBook);
            calculatedOrderBookDepth.Bids.Count.ShouldBe(3);
            var bidsList = calculatedOrderBookDepth.Bids;
            bidsList.ShouldNotBeNull();
            bidsList[0][0].ShouldBe(100);
            bidsList[0][1].ShouldBe(10);
            bidsList[1][0].ShouldBe(99);
            bidsList[1][1].ShouldBe(19);
            bidsList[2][0].ShouldBe(98);
            bidsList[2][1].ShouldBe(27);
        }
    }
}