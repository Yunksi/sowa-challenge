using System.Threading.Tasks;
using SowaLabsChallenge.Models;

namespace SowaLabsChallenge.Services.Calculation
{
    public interface ICalculationService
    {
        Task<OrderBookDepthDto> CalculateOrderBookDepth(OrderBook orderBook);
    }
}