using System;

namespace Starfruit.RouterLib;

public static class RoutableViewModelExtension
{
    public static void RegisterChild(this IRoutableViewModel parent, string segment, IRoutableViewModel child)
    {
        if (parent is null)
        {
            throw new ArgumentNullException(nameof(parent));
        }
        if (child is null)
        {
            throw new ArgumentNullException(nameof(child));
        }
        if (parent.RouteDefinition is null)
        {
            throw new InvalidOperationException($"Missing {nameof(parent)}'s {nameof(parent.RouteDefinition)}. Hints: The parent component must to register children on RouteDefinitionChanged");
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

    public static string GetRouteAddress(this IRoutableViewModel routableViewModel)
    {
        QueueStack<RouteSegment> routeSegments = new();
        var current = routableViewModel;

        while (current is not null)
        {
            if (current.RouteDefinition is null)
            {
                throw new InvalidOperationException($"{nameof(RouteNodeDefinition)} is not set. Hints: The parent component must to register children on RouteDefinitionChanged");
            }
            routeSegments.Prepend(new RouteSegment { SegmentName = current.RouteDefinition?.SegmentName, Parameters = current.RouteSegmentParameters });
            current = current.Parent;
        }

        return routeSegments.ToStringAddress();
    }
}