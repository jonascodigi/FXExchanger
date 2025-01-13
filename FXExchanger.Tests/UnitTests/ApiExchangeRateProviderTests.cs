using FluentAssertions;
using FXExchanger.Infrastructure.HttpClients;
using FXExchanger.Infrastructure.Providers;
using Moq;

namespace FXExchanger.Tests.UnitTests
{
    public class ApiExchangeRateProviderTests
    {
        [Fact]
        public void GetRate_ReturnsOne_WhenFromCurrencyEqualsToCurrency()
        {
            var mockClient = new Mock<IFreeCurrencyApiClient>();
            var provider = new ApiExchangeRateProvider(mockClient.Object);

            var result = provider.GetRate("USD", "usd");

            result.Should().Be(1m);

            mockClient.Verify(
                x => x.GetExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never
            );
        }

        [Fact]
        public void GetRate_ShouldThrow_WhenResponseIsNull()
        {
            var mockClient = new Mock<IFreeCurrencyApiClient>();
            mockClient
                .Setup(x => x.GetExchangeRatesAsync("USD", "EUR"))
                .ReturnsAsync((Dictionary<string, decimal>)null);

            var provider = new ApiExchangeRateProvider(mockClient.Object);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                provider.GetRate("USD", "EUR")
            );

            ex.Message.Should().Contain("Failed to retrieve exchange rates for base");
        }

        [Fact]
        public void GetRate_ShouldThrow_WhenResponseDoesNotContainToCurrency()
        {
            var mockClient = new Mock<IFreeCurrencyApiClient>();
            mockClient
                .Setup(x => x.GetExchangeRatesAsync("USD", "JPY"))
                .ReturnsAsync(new Dictionary<string, decimal>
                {
                { "EUR", 0.85m }
                });

            var provider = new ApiExchangeRateProvider(mockClient.Object);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                provider.GetRate("USD", "JPY")
            );

            ex.Message.Should().Contain("Unable to find a rate from 'USD' to 'JPY'");
        }

        [Fact]
        public void GetRate_ReturnsCorrectValue_WhenResponseContainsToCurrency()
        {
            var mockClient = new Mock<IFreeCurrencyApiClient>();
            mockClient
                .Setup(x => x.GetExchangeRatesAsync("USD", "EUR"))
                .ReturnsAsync(new Dictionary<string, decimal>
                {
                { "EUR", 0.85m }
                });

            var provider = new ApiExchangeRateProvider(mockClient.Object);

            var result = provider.GetRate("USD", "EUR");

            result.Should().Be(0.85m);
        }
    }
}
