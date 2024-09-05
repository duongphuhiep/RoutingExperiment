using DemoRoutingApp.ViewModels;

namespace DemoRoutingApp.Models;

public static class Router
{
    static readonly RootRouteDefinition<MainViewModel> Root = new()
    {
        Children = [
            new RouteDefinition<WalletsViewModel> {
                Path = "wallets"
            },
            new RouteDefinition<TransfersViewModel> {
                Path = "transfers"
            }
        ]
    };

    //public static void Goto(string path)
    //{
    //    var queue = RouteParser.Parse(path, Root);
    //    foreach (var routeData in queue)
    //    {
    //        var component = App.ServiceProvider.GetService(routeData.Component);
    //    }
    //}
}
