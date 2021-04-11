using Microsoft.Extensions.DependencyInjection;
using SowaLabsChallenge.Services.Calculation;
using SowaLabsChallenge.Services.FetchData;

namespace SowaLabsChallenge.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IFetchDataService, FetchDataService>();
            services.AddSingleton<ICalculationService, CalculationService>();
        }
    }
}