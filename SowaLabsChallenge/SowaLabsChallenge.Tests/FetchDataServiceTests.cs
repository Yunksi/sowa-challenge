using System;
using System.Threading.Tasks;
using Shouldly;
using SowaLabsChallenge.Services.FetchData;
using Xunit;

namespace SowaLabsChallenge.Tests
{
    public class FetchDataServiceTests
    {
        private readonly IFetchDataService _fetchDataService;

        public FetchDataServiceTests(IFetchDataService fetchDataService)
        {
            _fetchDataService = fetchDataService;
        }
        
        [Fact]
        public async Task FetchDataFromUrl_ShouldThrowExceptionIfInvalidUrl()
        {
            await Should.ThrowAsync<Exception>(_fetchDataService.FetchOrderBookDataFromUrl("test"));
        }
        
        [Fact]
        public async Task FetchDataFromUrl_ShouldReturnDeserializedObject()
        {
            var orderBook = await _fetchDataService.FetchOrderBookDataFromUrl("https://www.bitstamp.net/api/v2/order_book/btceur");
            orderBook.ShouldNotBeNull();
        }
    }
}
