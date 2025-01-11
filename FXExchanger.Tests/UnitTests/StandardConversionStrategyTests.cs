using FXExchanger.Core.Services;
using FXExchanger.Core.Strategies;
using FXExchanger.Infrastructure.Providers;

namespace FXExchanger.Tests.UnitTests
{
    public class StandardConversionStrategyTests
    {
        [Fact]
        public void ConvertCurrency_SameCurrency_ReturnsSameValue()
        {
            var mockRateProvider = new MockExchangeRateProvider();
            var strategy = new StandardConversionStrategy(mockRateProvider);
            var converter = new CurrencyConverter(strategy);

            var result = converter.Convert(100m, "USD", "USD");

            Assert.Equal(100m, result);
        }
    }
}
