namespace DemoRoutingApp.Models;

public interface IRoutableViewModel
{
    RouteNodeDefinition? RouteDefinition { get; }
    /// <summary>
    /// Attach the current View Model to the given route definition. This method is usually called by the parent view model.
    /// </summary>
    /// <param name="routeDefinition"></param>
    void AttachToRouteDefinition(RouteNodeDefinition routeDefinition);


    /// <summary>
    /// Attache all children of the current View Models to their corresponding children of the current Route Definition.
    /// This method is usually called each time the current route definition is changed.
    /// </summary>
    void AttachChildrenToRouteDefinitions();


    void OnRouteChanged(RouteChangedEvent e);
}

