using FXExchanger.ConsoleApp.Application;
using FXExhanger;
using Microsoft.Extensions.DependencyInjection;

static class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        var startup = new Startup();
        startup.ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var uiHandler = serviceProvider.GetService<ConsoleHandler>();

        if (uiHandler == null)
        {
            Console.WriteLine("Failed to resolve ConsoleHandler. Exiting...");
            return;
        }

        uiHandler.Run();
    }
}










