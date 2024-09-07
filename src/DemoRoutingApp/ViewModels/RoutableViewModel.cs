using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.Models;
using System;

namespace DemoRoutingApp.ViewModels;

public abstract partial class RoutableViewModel : ViewModelBase, IRoutableViewModel
{
    [ObservableProperty]

    private RouteNodeDefinition? _routeDefinition;

    public void AttachToRouteDefinition(RouteNodeDefinition routeDefinition)
    {
        ArgumentNullException.ThrowIfNull(routeDefinition);
        routeDefinition.RegisterComponent(this);
        RouteDefinition = routeDefinition;
    }

    partial void OnRouteDefinitionChanged(RouteNodeDefinition? routeDefinition) => AttachChildrenToRouteDefinitions();

    public abstract void AttachChildrenToRouteDefinitions();
    public abstract void OnRouteChanged(RouteChangedEvent e);
}
