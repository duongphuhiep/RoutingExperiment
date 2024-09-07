using DemoRoutingApp.ViewModels;

namespace DemoRoutingApp.Models;

public static class RouterConfig
{
    public static readonly RouteNodeDefinition Root = new RouteNodeDefinition<MainViewModel>("/");
    public static readonly Navigator Navigator = new Navigator(Root);

    static RouterConfig()
    {
        Root.AddChild(new RouteNodeDefinition<TransfersViewModel>("transfers"));
        Root.AddChild(new RouteNodeDefinition<WalletsViewModel>("wallets"));
    }
}
