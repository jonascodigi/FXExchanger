using FXExchanger.Core.Interfaces;
using FXExchanger.Shared.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace FXExchanger.Infrastructure.Providers
{
    public class CachingExchangeProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateProvider _innerProvider;
        private readonly IMemoryCache _cache;
        private readonly AppSettings _appSettings;

        public CachingExchangeProvider(
            IExchangeRateProvider innerProvider, IMemoryCache cache, AppSettings appSettings)
        {
            _innerProvider = innerProvider;
            _cache = cache;
            _appSettings = appSettings;
        }

        public decimal GetRate(string fromCurrency, string toCurrency)
        {
            var cacheKey = $"{fromCurrency}:{toCurrency}";

            if (!_cache.TryGetValue(cacheKey, out decimal rate))
            {
                rate = _innerProvider.GetRate(fromCurrency, toCurrency);

                _cache.Set(cacheKey, rate, TimeSpan.FromMinutes(_appSettings.CacheDurationMinutes));
            }
            return rate;
        }
    }
}
