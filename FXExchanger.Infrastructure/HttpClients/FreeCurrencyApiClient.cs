using FXExchanger.Infrastructure.DTOs;
using FXExchanger.Shared.Configuration;
using Newtonsoft.Json;

namespace FXExchanger.Infrastructure.HttpClients
{
    public class FreeCurrencyApiClient : IFreeCurrencyApiClient
    {
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;

        public FreeCurrencyApiClient(HttpClient httpClient, AppSettings appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync(string fromCurrency, string toCurrency)
        {
            if (string.IsNullOrWhiteSpace(fromCurrency))
                throw new ArgumentNullException(nameof(fromCurrency));
            if (string.IsNullOrWhiteSpace(toCurrency))
                throw new ArgumentNullException(nameof(toCurrency));

            var endpoint = $"v1/latest?base_currency={fromCurrency}&currencies={toCurrency}";

            _httpClient.DefaultRequestHeaders.Remove("apikey");
            _httpClient.DefaultRequestHeaders.Add("apikey", _appSettings.ApiKey);

            var response = await _httpClient.GetAsync(endpoint).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch exchange rates. Status code: {response.StatusCode}");
            }

            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var data = JsonConvert.DeserializeObject<ExchangeRateResponse>(responseBody);

            if (data?.Data == null)
            {
                throw new InvalidOperationException("Failed to fetch exchange rates from API.");
            }

            return data.Data;
        }
    }
}
