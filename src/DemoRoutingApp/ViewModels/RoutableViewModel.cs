using CommunityToolkit.Mvvm.ComponentModel;
using Starfruit.RouterLib;

namespace DemoRoutingApp.ViewModels;

public abstract partial class RoutableViewModel : ViewModelBase, IRoutableViewModel
{
    [ObservableProperty]

    private IRoutableViewModel? _parent;

    [ObservableProperty]

    private RouteNodeDefinition? _routeDefinition;

    [ObservableProperty]
    private UnorderedKeyValueCollection _routeSegmentParameters;

    partial void OnRouteDefinitionChanged(RouteNodeDefinition? routeDefinition) => RegisterChildren();

    public abstract void RegisterChildren();
    public abstract void OnRouteChanged(RouteSegmentChangedEvent e);
}
