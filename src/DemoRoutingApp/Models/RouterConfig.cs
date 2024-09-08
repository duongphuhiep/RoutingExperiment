using DemoRoutingApp.ViewModels;

namespace DemoRoutingApp.Models;

public static class RouterConfig
{
    public static readonly RouteNodeDefinition Root =
        new RouteNodeDefinition<MainViewModel>("/", [
            new RouteNodeDefinition<TransfersViewModel>("transfers"),
            new RouteNodeDefinition<WalletsViewModel>("personalWallets", [
                new RouteNodeDefinition<WalletDetailViewModel>("walletDetails")
            ]),
            new RouteNodeDefinition<WalletsViewModel>("companyWallets", [
                new RouteNodeDefinition<WalletDetailViewModel>("walletDetails")
            ])
        ]);
    public static readonly INavigator Navigator = new Navigator(Root);
}
