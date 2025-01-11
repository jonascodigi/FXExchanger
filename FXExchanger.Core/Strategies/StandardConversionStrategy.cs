using FXExchanger.Core.Interfaces;

namespace FXExchanger.Core.Strategies
{
    public class StandardConversionStrategy : ICurrencyConversionStrategy
    {
        private readonly IExchangeRateProvider _rateProvider;
       
        public StandardConversionStrategy(IExchangeRateProvider rateProvider)
        {
            _rateProvider = rateProvider;
        }

        public decimal Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            var rate = _rateProvider.GetRate(fromCurrency, toCurrency);
            return amount * rate;
        }
    }
}
