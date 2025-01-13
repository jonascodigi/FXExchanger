using FXExchanger.Core.Interfaces;
using FXExchanger.Infrastructure.HttpClients;

namespace FXExchanger.Infrastructure.Providers
{
    public class ApiExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IFreeCurrencyApiClient _freeCurrencyApiClient;

        public ApiExchangeRateProvider(IFreeCurrencyApiClient freeCurrencyApiClient)
        {
            _freeCurrencyApiClient = freeCurrencyApiClient;
        }

        public decimal GetRate(string fromCurrency, string toCurrency)
        {
            if (string.Equals(fromCurrency, toCurrency, StringComparison.OrdinalIgnoreCase))
            {
                return 1m;
            }

            var response = _freeCurrencyApiClient.GetExchangeRatesAsync(fromCurrency, toCurrency).GetAwaiter().GetResult();

            if (response == null)
            {
                throw new InvalidOperationException($"Failed to retrieve exchange rates for base '{fromCurrency}' from freecurrencyapi.");
            }

            if (!response.ContainsKey(toCurrency))
            {
                throw new InvalidOperationException(
                    $"Unable to find a rate from '{fromCurrency}' to '{toCurrency}' in the API response."
                );
            }

            return response[toCurrency];
        }
    }
}
