using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SowaLabsChallenge.Models;

namespace SowaLabsChallenge.Services.FetchData
{
    public class FetchDataService: IFetchDataService
    {
        private readonly IHttpClientFactory _client;

        public FetchDataService(IHttpClientFactory client)
        {
            _client = client;
        }
        public async Task<Models.OrderBook> FetchOrderBookDataFromUrl(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var httpClient = _client.CreateClient();

            var httpResponse = await httpClient.SendAsync(request);

            if (!httpResponse.IsSuccessStatusCode) return null;
            await using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
            var orderBook = await JsonSerializer.DeserializeAsync<Models.OrderBook>(responseStream);
            return orderBook;

        }
    }
}