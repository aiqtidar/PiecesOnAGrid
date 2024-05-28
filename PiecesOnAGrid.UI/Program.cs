using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PiecesOnAGrid.Service.GameEngineService;
using PiecesOnAGrid.UI;

public class Program
{
    public static void Main(string[] args)
    {
        // Just a dummy read operation for now
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("config.json", optional: false, reloadOnChange: true)
            .Build();

        // COnfigure services
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        
        // Build and run
        serviceCollection.BuildServiceProvider().GetService<ConsoleApplication>()?.Run(configuration);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IGameEngine<int>, GameEngine<int>>();
        services.AddTransient<ConsoleApplication>();
        
    }
}