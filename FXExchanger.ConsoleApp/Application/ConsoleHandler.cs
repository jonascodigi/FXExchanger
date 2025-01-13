using FXExchanger.Core.Interfaces;
using FXExchanger.Core.Services;
using System.Text.RegularExpressions;

namespace FXExchanger.ConsoleApp.Application
{
    public class ConsoleHandler
    {
        private readonly CurrencyConverter _currencyConverter;
        private readonly ICurrencyValidator _currencyValidator;

        private const string ExchangeCommandPattern = @"Exchange\s+(\w{3})/(\w{3})\s+(\d+(\.\d+)?)";

        public ConsoleHandler(CurrencyConverter currencyConverter, ICurrencyValidator currencyValidator)
        {
            _currencyConverter = currencyConverter;
            _currencyValidator = currencyValidator;
        }

        public void Run()
        {
            ShowIntro();

            if (_currencyConverter == null)
            {
                Console.WriteLine("CurrencyConverter service is not available.");
                return;
            }

            while (true)
            {
                var input = GetUserInput();
                if (IsExitCommand(input))
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }

                if (!TryParseExchangeCommand(input, out var fromCurrency, out var toCurrency, out var amount))
                {
                    ShowInvalidCommand();
                    continue;
                }

                if (!ValidateCurrencies(fromCurrency, toCurrency))
                {
                    continue;
                }
                ExecuteExchange(fromCurrency, toCurrency, amount);
            }
        }

        private void ShowIntro()
        {
            Console.WriteLine("FXExchanger Application");
            Console.WriteLine("Usage: Exchange <currency pair> <amount to exchange> OR 'exit' to quit.");
        }
        private string GetUserInput()
        {
            Console.Write("\n> ");
            var input = Console.ReadLine();
            return input ?? string.Empty;
        }
        private bool IsExitCommand(string input)
        {
            return string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase);
        }
        private bool TryParseExchangeCommand(string input, out string fromCurrency, out string toCurrency, out decimal amount)
        {
            fromCurrency = string.Empty;
            toCurrency = string.Empty;
            amount = 0m;

            Match match = Regex.Match(input, ExchangeCommandPattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                return false;
            }

            fromCurrency = match.Groups[1].Value.ToUpper();
            toCurrency = match.Groups[2].Value.ToUpper();

            return decimal.TryParse(match.Groups[3].Value, out amount);
        }
        private void ShowInvalidCommand()
        {
            Console.WriteLine("Input string does not match the expected format.");
            Console.WriteLine("Usage: Exchange <currency pair> <amount to exchange> OR 'exit' to quit.");
        }
        private bool ValidateCurrencies(string fromCurrency, string toCurrency)
        {
            if (!_currencyValidator.IsValidIsoCode(fromCurrency))
            {
                Console.WriteLine($"'{fromCurrency}' is either not a valid ISO code or not supported by this application.");
                return false;
            }
            if (!_currencyValidator.IsValidIsoCode(toCurrency))
            {
                Console.WriteLine($"'{toCurrency}' is either not a valid ISO code or not supported by this application.");
                return false;
            }
            return true;
        }
        private void ExecuteExchange(string fromCurrency, string toCurrency, decimal amount)
        {
            try
            {
                var result = _currencyConverter.Convert(amount, fromCurrency, toCurrency);
                Console.WriteLine($"{result}");
            } catch (Exception ex)
            {
                Console.WriteLine($"Error performing exchange: {ex.Message}");
            }
        }
    }
}
