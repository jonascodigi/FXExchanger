using FXExchanger.Core.Interfaces;

namespace FXExchanger.Core.Services
{
    public class IsoCurrencyValidator : ICurrencyValidator
    {
        private static readonly HashSet<string> ValidCodes = new HashSet<string>
        {
            "DKK", "EUR", "USD", "GBP", "SEK", "NOK", "CHF", "JPY",
        };
        public bool IsValidIsoCode(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
                return false;
            return ValidCodes.Contains(currencyCode.ToUpper());
        }
    }
}
