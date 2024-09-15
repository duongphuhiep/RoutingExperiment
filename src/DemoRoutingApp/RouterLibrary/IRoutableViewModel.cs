using System.Collections.Specialized;

namespace DemoRoutingApp.Models;

public interface IRoutableViewModel
{
    RouteNodeDefinition? RouteDefinition { get; set; }

    /// <summary>
    /// Attach all children of the current View Models to their corresponding children of the current Route Definition.
    /// This method is usually called each time the current route definition is changed.
    /// </summary>
    void RegisterChildren();

    /// <summary>
    /// Display the appropriated child View Model correspond to the <see cref="RouteChangedEvent.NextChildNode"/>
    /// </summary>
    void OnRouteChanged(RouteChangedEvent e);

    /// <summary>
    /// Parameters of the current View Model.
    /// </summary>
    NameValueCollection RouteSegmentParameters { get; }
}

