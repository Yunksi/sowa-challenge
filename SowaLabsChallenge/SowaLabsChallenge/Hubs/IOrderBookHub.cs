using System.Threading.Tasks;
using SowaLabsChallenge.Models;

namespace SowaLabsChallenge.Hubs
{
    public interface IOrderBookHub
    {
        Task UpdateOrderBook(OrderBookDepthDto orderBookDepth);
    }
}