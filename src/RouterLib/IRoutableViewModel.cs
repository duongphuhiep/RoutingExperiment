namespace Starfruit.RouterLib;

public interface IRoutableViewModel
{
    IRoutableViewModel? Parent { get; set; }

    RouteNodeDefinition? RouteDefinition { get; set; }

    /// <summary>
    /// Set the RoutDefinition and the Parent:
    /// Attach all children of the current IRoutableViewModel to their corresponding children.
    /// This method is usually called each time the current route definition is changed.
    /// </summary>
    void RegisterChildren();

    /// <summary>
    /// Display the appropriated child View Model correspond to the <see cref="RouteSegmentChangedEvent.NextChildNode"/>
    /// </summary>
    void OnRouteChanged(RouteSegmentChangedEvent e);

    /// <summary>
    /// Parameters of the current View Model.
    /// </summary>
    UnorderedKeyValueCollection? RouteSegmentParameters { get; set; }
}
