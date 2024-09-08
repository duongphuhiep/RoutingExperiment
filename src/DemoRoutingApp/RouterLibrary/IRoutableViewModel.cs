namespace DemoRoutingApp.Models;

public interface IRoutableViewModel
{
    RouteNodeDefinition? RouteDefinition { get; set; }

    /// <summary>
    /// Attache all children of the current View Models to their corresponding children of the current Route Definition.
    /// This method is usually called each time the current route definition is changed.
    /// </summary>
    void RegisterChildren();

    void OnRouteChanged(RouteChangedEvent e);
}

