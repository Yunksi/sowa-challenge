using System.Threading.Tasks;

namespace SowaLabsChallenge.Hubs
{
    public interface IOrderBookHub
    {
        Task UpdateOrderBook(string orderBookDepth);
    }
}