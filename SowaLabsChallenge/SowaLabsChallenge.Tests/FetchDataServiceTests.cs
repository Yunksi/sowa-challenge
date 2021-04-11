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
        public void Test1()
        {

        }
    }
}
