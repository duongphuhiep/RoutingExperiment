using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using DemoRoutingApp.BusinessLogic;
using DemoRoutingApp.ViewModels;
using DemoRoutingApp.Views;
//using HotAvalonia;
using Microsoft.Extensions.DependencyInjection;
using Starfruit.RouterLib;
using System;
using System.Linq;

namespace DemoRoutingApp;

public partial class App : Application, IRecipient<RefreshApplicationRequest>
{
    public static readonly IServiceProvider ServiceProvider = BuildDependencyGraph()
        .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

    public static ServiceCollection BuildDependencyGraph()
    {
        ServiceCollection services = new();
        services.AddLoggingService();
        services.AddRouting();
        services.RegisterViewModels();
        services.RegisterViews();
        services.RegisterBusinessLogic();

        return services;
    }

    public override void Initialize()
    {
        //this.EnableHotReload();
        AvaloniaXamlLoader.Load(this);
    }

    private IDataContextProvider _rootView;

    public override void OnFrameworkInitializationCompleted()
    {
        DisableAvaloniaDataAnnotationValidation();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>()
            };
            _rootView = desktop.MainWindow;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
            };
            _rootView = singleViewPlatform.MainView;
        }
        WeakReferenceMessenger.Default.RegisterAll(this);
        base.OnFrameworkInitializationCompleted();
    }

    private static void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    public void Receive(RefreshApplicationRequest message)
    {
        if (_rootView is null)
        {
            throw new InvalidOperationException("Framework Initialization is not yet Completed");
        }
        if (_rootView.DataContext is MainWindowViewModel)
        {
            _rootView.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
        }
        else if (_rootView.DataContext is MainViewModel)
        {
            _rootView.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}
