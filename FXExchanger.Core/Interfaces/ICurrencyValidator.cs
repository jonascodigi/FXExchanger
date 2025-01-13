namespace FXExchanger.Core.Interfaces
{
    public interface ICurrencyValidator
    {
        bool IsValidIsoCode(string currencyCode);
    }
}
