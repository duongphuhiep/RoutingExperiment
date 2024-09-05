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
    public static ServiceCollection RegisterViewModels(this ServiceCollection services)
    {
        services.AddSingleton<AddressBarViewModel>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<WalletsViewModel>();
        services.AddSingleton<TransfersViewModel>();
        return services;
    }

    /// <summary>
    /// Registration for application
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static ServiceCollection RegisterViews(this ServiceCollection services)
    {
        //all routeable view should be registered as scoped
        services.AddScoped<AddressBar>();
        services.AddScoped<MainWindow>();
        services.AddScoped<MainView>();
        services.AddScoped<WalletsView>();
        services.AddScoped<TransfersView>();

        //other view should be registered as transient
        return services;
    }
}
