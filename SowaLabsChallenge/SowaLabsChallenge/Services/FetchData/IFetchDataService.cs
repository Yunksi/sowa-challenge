using System.Threading.Tasks;
using SowaLabsChallenge.Models;

namespace SowaLabsChallenge.Services.FetchData
{
    public interface IFetchDataService
    {
        Task<OrderBook> FetchOrderBookDataFromUrl(string url);
    }
}