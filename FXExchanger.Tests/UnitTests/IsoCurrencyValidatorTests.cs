using FluentAssertions;
using FXExchanger.Core.Services;

namespace FXExchanger.Tests.UnitTests
{
    public class IsoCurrencyValidatorTests
    {
        [Theory]
        [InlineData("EUR")]
        [InlineData("USD")]
        [InlineData("GBP")]
        [InlineData("SEK")]
        public void IsValidIsoCode_ReturnsTrue_ForKnownValidCodes(string code)
        {
            var validator = new IsoCurrencyValidator();

            var result = validator.IsValidIsoCode(code);

            result.Should().BeTrue($"'{code}' is in the predefined set of valid codes.");
        }

        [Theory]
        [InlineData("ABC")]
        [InlineData("XYZ")]
        [InlineData(null)]
        public void IsValidIsoCode_ReturnsFalse_ForInvalidCodes(string code)
        {
            var validator = new IsoCurrencyValidator();

            var result = validator.IsValidIsoCode(code);

            result.Should().BeFalse($"'{code}' should not be recognized as valid.");
        }
    }
}
