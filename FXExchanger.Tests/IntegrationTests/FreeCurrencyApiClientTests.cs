using FXExchanger.Infrastructure.HttpClients;
using FXExchanger.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using FluentAssertions;

namespace FXExchanger.Tests.IntegrationTests
{
    public class FreeCurrencyApiClientTests
    {
        private readonly IFreeCurrencyApiClient _client;

        public FreeCurrencyApiClientTests()
        {
            var configuration = BuildConfiguration();

            var appSettings = configuration
                .GetSection("AppSettings")
                .Get<AppSettings>()
                ?? throw new InvalidOperationException("AppSettings not found or invalid.");

            if (string.IsNullOrWhiteSpace(appSettings.ApiKey))
            {
                throw new InvalidOperationException("API key is missing. " +
                    "Make sure you have set it in appsettings.json file");
            }

            var httpClient = new HttpClient {
                BaseAddress = new Uri(appSettings.ExchangeRatesApiUrl)
            };

            _client = new FreeCurrencyApiClient(httpClient, appSettings);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldReturnRates_ForValidCurrencies()
        {
            var fromCurrency = "USD";
            var toCurrency = "EUR";

            var result = await _client.GetExchangeRatesAsync(fromCurrency, toCurrency);

            result.Should().ContainKey("EUR");
            result["EUR"].Should().BeGreaterThan(0, because: "A valid exchange rate should be returned for USD to EUR");
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldThrowException_ForInvalidCurrency()
        {
            var fromCurrency = "XYZ"; 
            var toCurrency = "EUR";

            await Assert.ThrowsAsync<HttpRequestException>(async () => {
                await _client.GetExchangeRatesAsync(fromCurrency, toCurrency);
            });
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
