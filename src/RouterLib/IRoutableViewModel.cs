using System;
using System.Collections.Specialized;

namespace Starfruit.RouterLib;

public interface IRoutableViewModel
{
    IRoutableViewModel Parent { get; set; }

    RouteNodeDefinition? RouteDefinition { get; set; }

    /// <summary>
    /// Set the RoutDefinition and the Parent:
    /// Attach all children of the current IRoutableViewModel to their corresponding children.
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

public static class RoutableViewModelExtension
{
    public static void RegisterChild(this IRoutableViewModel parent, string segment, IRoutableViewModel child)
    {
        if (parent is null)
        {
            throw new ArgumentNullException(nameof(parent));
        }
        if (string.IsNullOrEmpty(segment))
        {
            throw new ArgumentNullException(nameof(segment));
        }
        if (child is null)
        {
            throw new ArgumentNullException(nameof(child));
        }
        if (parent.RouteDefinition is null)
        {
            throw new InvalidOperationException($"Missing parent's {nameof(parent.RouteDefinition)}. Hints: Make sure to register children on RouteDefinitionChanged");
        }
        var childRouteDefinition = parent.RouteDefinition[segment];
        if (childRouteDefinition is null)
        {
            throw new InvalidOperationException($"Child segment '{segment}' is not defined in the parent's {parent.RouteDefinition}");
        }
        childRouteDefinition.RegisterComponent(child);
        child.Parent = parent;
    }

    public static void RegisterAsRoot(this IRoutableViewModel me, RouteNodeDefinition root)
    {
        if (!root.IsRoot)
        {
            throw new ArgumentException($"{nameof(RouteNodeDefinition)} is not a root");
        }
        root.RegisterComponent(me);
    }
}