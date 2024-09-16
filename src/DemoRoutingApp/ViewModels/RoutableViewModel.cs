using CommunityToolkit.Mvvm.ComponentModel;
using Starfruit.RouterLib;
using System.Collections.Specialized;

namespace DemoRoutingApp.ViewModels;

public abstract partial class RoutableViewModel : ViewModelBase, IRoutableViewModel
{
    [ObservableProperty]

    private IRoutableViewModel? _parent;

    [ObservableProperty]

    private RouteNodeDefinition? _routeDefinition;

    [ObservableProperty]
    private NameValueCollection _routeSegmentParameters;

    partial void OnRouteDefinitionChanged(RouteNodeDefinition? routeDefinition) => RegisterChildren();

    public abstract void RegisterChildren();
    public abstract void OnRouteChanged(RouteChangedEvent e);
}
