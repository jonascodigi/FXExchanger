using FXExchanger.Core.Interfaces;

namespace FXExchanger.Infrastructure.Providers
{
    public class MockExchangeRateProvider : IExchangeRateProvider
    {
        private readonly Dictionary<string, decimal> _rates = new()
        {
            {"DKK:EUR", 7.4394m },
            {"DKK:USD", 6.6311m },
            {"DKK:GBP", 8.5285m },
            {"DKK:SEK", 0.7610m },
            {"DKK:NOK", 0.7840m },
            {"DKK:CHF", 6.8358m },
            {"DKK:JPY", 0.05974m }
        };
        public decimal GetRate(string fromCurrency, string toCurrency)
        {
            var key = $"{fromCurrency}:{toCurrency}";
            if (_rates.ContainsKey(key))
                return _rates[key];
            throw new InvalidOperationException("Rate not found");
        }
    }
}
