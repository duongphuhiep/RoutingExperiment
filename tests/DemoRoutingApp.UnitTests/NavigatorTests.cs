using DemoRoutingApp.Models;
using DemoRoutingApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace DemoRoutingApp.UnitTests;

public class NavigatorTests
{

    [Fact]
    public void GotoTest()
    {
        var mainViewModel = App.ServiceProvider.GetRequiredService<MainViewModel>();
        var navigator = App.ServiceProvider.GetRequiredService<INavigator>();
        navigator.Goto("/wallets");
        mainViewModel.SelectedTabValue.Content.ShouldBeOfType<WalletsViewModel>();
        navigator.Goto("/transfers");
        mainViewModel.SelectedTabValue.Content.ShouldBeOfType<TransfersViewModel>();

    }
}
