using FluentAssertions;
using FXExchanger.Core.Interfaces;
using FXExchanger.Core.Strategies;
using Moq;

namespace FXExchanger.Tests.UnitTests
{
    public class StandardConversionStrategyTests
    {
        [Fact]
        public void Convert_ShouldMultiplyAmountByRate_FromProvider()
        {
            var mockProvider = new Mock<IExchangeRateProvider>();
            mockProvider
                .Setup(p => p.GetRate("USD", "EUR"))
                .Returns(0.85m);

            var strategy = new StandardConversionStrategy(mockProvider.Object);

            var result = strategy.Convert(100m, "USD", "EUR");

            result.Should().Be(85m);
            mockProvider.Verify(p => p.GetRate("USD", "EUR"), Times.Once);
        }

        [Fact]
        public void Convert_ShouldCallProviderWithGivenCurrencies()
        {
            var mockProvider = new Mock<IExchangeRateProvider>();
            var strategy = new StandardConversionStrategy(mockProvider.Object);

            strategy.Convert(200m, "GBP", "JPY");

            mockProvider.Verify(p => p.GetRate("GBP", "JPY"), Times.Once);
        }
    }
}
