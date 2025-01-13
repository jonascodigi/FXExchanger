using FluentAssertions;
using FXExchanger.Infrastructure.DTOs;
using Newtonsoft.Json;

namespace FXExchanger.Tests.UnitTests
{
    public class ExchangeRateResponseTests
    {
        [Fact]
        public void ShouldDeserializeDataFieldCorrectly()
        {
            var json = @"{
              ""data"": {
                ""EUR"": 0.92,
                ""GBP"": 0.78
              }
            }";
            var result = JsonConvert.DeserializeObject<ExchangeRateResponse>(json);

            result.Should().NotBeNull();
            result!.Data.Should().ContainKey("EUR");
            result.Data["EUR"].Should().Be(0.92m);
            result.Data["GBP"].Should().Be(0.78m);
        }

        [Fact]
        public void ShouldHandleMissingDataField()
        {
            var json = @"{}";

            var result = JsonConvert.DeserializeObject<ExchangeRateResponse>(json);

            result.Should().NotBeNull();
            result!.Data.Should().NotBeNull();
        }
    }
}
