using FXExchanger.Core.Interfaces;

namespace FXExchanger.Core.Services
{
    public class CurrencyConverter
    {
        private readonly ICurrencyConversionStrategy _currencyConversionStrategy;
        
        public CurrencyConverter(ICurrencyConversionStrategy currencyConversionStrategy)
        {
            _currencyConversionStrategy = currencyConversionStrategy;
        }

        public decimal Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            return _currencyConversionStrategy.Convert(amount, fromCurrency, toCurrency);
        }
    }
}
