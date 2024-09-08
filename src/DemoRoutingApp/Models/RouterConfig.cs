using DemoRoutingApp.ViewModels;

namespace DemoRoutingApp.Models;

public static class RouterConfig
{
    public static readonly RouteNodeDefinition Root = new RouteNodeDefinition<MainViewModel>("/",
        new RouteNodeDefinition<TransfersViewModel>("transfers"),
            new RouteNodeDefinition<WalletsViewModel>("personalWallets"),
            new RouteNodeDefinition<WalletsViewModel>("companyWallets")
        );
    public static readonly INavigator Navigator = new Navigator(Root);
}
