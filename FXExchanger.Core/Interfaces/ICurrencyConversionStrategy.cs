namespace FXExchanger.Core.Interfaces
{
    public interface ICurrencyConversionStrategy
    {
        decimal Convert(decimal amount, string fromCurrency, string toCurrency);
    }
}
