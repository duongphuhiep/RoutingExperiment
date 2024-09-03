using DemoRoutingApp.ViewModels;
using DemoRoutingApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DemoRoutingApp;

internal static class Ioc
{
    /// <summary>
    /// Registration for Microsoft Logging framework
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddLoggingService(this IServiceCollection services)
    {
        services.AddLogging();
        return services;
    }

    /// <summary>
    /// Registration for application
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static ServiceCollection RegisterApplicationServices(this ServiceCollection services)
    {
        services.AddSingleton<AddressBarViewModel>();
        services.AddTransient<AddressBar>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<MainWindow>();

        services.AddSingleton<MainViewModel>();
        services.AddTransient<MainView>();

        return services;
    }
}
