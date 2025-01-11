using FXExchanger.Core.Services;
using FXExhanger;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

Console.WriteLine("FXExchanger Application");




var startup = new Startup();
var serviceProvider = startup.ConfigureServices();
var currencyConverter = serviceProvider.GetService<CurrencyConverter>();

if (currencyConverter == null)
{
    Console.WriteLine("CurrencyConverter service is not available.");
    return;
}


string pattern = @"Exchange\s+(\w{3})/(\w{3})\s+(\d+(\.\d+)?)";


Console.WriteLine("Usage: Exchange <currency pair> <amount to exchange>");
var input = Console.ReadLine();
if (string.IsNullOrWhiteSpace(input))
{
    Console.WriteLine("Invalid input. Please provide a valid exchange format.");
    return;
}
Match match = Regex.Match(input, pattern);

if (match.Success)
{
    string fromCurrency = match.Groups[1].Value;
    string toCurrency = match.Groups[2].Value;
    decimal amount = decimal.Parse(match.Groups[3].Value);

    var result = currencyConverter.Convert(amount, fromCurrency, toCurrency);

    Console.WriteLine($"{result}");
}
else
{
    Console.WriteLine("Input string does not match the expected format.");
}



