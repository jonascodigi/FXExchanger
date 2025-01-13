using FluentAssertions;
using FXExchanger.Core.Interfaces;
using FXExchanger.Infrastructure.Providers;
using FXExchanger.Shared.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace FXExchanger.Tests.UnitTests
{
    public class CachingExchangeProviderTests
    {

        private readonly AppSettings _testAppSettings;
        private readonly IMemoryCache _memoryCache;

        public CachingExchangeProviderTests()
        {
            _testAppSettings = new AppSettings {
                CacheDurationMinutes = 15,
            };

            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public void GetRate_ShouldReturnCachedValue_WhenCacheKeyExists()
        {
            var innerProviderMock = new Mock<IExchangeRateProvider>();
            innerProviderMock
                .Setup(p => p.GetRate("USD", "EUR"))
                .Returns(0.85m);

            var cachingProvider = new CachingExchangeProvider(innerProviderMock.Object, _memoryCache, _testAppSettings);

            var firstResult = cachingProvider.GetRate("USD", "EUR");

            var secondResult = cachingProvider.GetRate("USD", "EUR");

            firstResult.Should().Be(0.85m);
            secondResult.Should().Be(0.85m);

            innerProviderMock.Verify(p => p.GetRate("USD", "EUR"), Times.Once);
        }

        [Fact]
        public void GetRate_ShouldCallInnerProvider_WhenCacheIsEmptyOrExpired()
        {
            var innerProviderMock = new Mock<IExchangeRateProvider>();
            innerProviderMock
                .Setup(p => p.GetRate("GBP", "JPY"))
                .Returns(150m);

            var cachingProvider = new CachingExchangeProvider(innerProviderMock.Object, _memoryCache, _testAppSettings);

            var result = cachingProvider.GetRate("GBP", "JPY");

            result.Should().Be(150m);

            innerProviderMock.Verify(p => p.GetRate("GBP", "JPY"), Times.Once);
        }
    }
}
