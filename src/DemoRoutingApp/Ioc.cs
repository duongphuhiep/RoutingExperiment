using DemoRoutingApp.Models;
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
        services.AddTransient<AddressBarViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<WalletsViewModel>();
        services.AddTransient<TransfersViewModel>();
        services.AddTransient<WalletDetailViewModel>();
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
        services.AddTransient<AddressBar>();
        services.AddTransient<MainWindow>();
        services.AddTransient<MainView>();
        services.AddTransient<WalletsView>();
        services.AddTransient<TransfersView>();
        services.AddTransient<WalletDetailView>();

        //other view should be registered as transient
        return services;
    }

    public static ServiceCollection AddRouting(this ServiceCollection services)
    {
        services.AddSingleton(RouterConfig.Navigator);
        return services;
    }
}
