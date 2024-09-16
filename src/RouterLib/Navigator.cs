using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Starfruit.RouterLib;

public interface INavigator
{
    void Goto(string path);
}

public class Navigator : INavigator
{
    private readonly RouteNodeDefinition _root;

    public Navigator(RouteNodeDefinition root)
    {
        _root = root;
    }

    public void Goto(string path)
    {
        try
        {
            foreach (var e in GetEventsForPath(path))
            {
                e.CurrentNode.CleanDeadComponents();
                foreach (var cachedComponent in e.CurrentNode.Components)
                {
                    cachedComponent.TryGetTarget(out var component);
                    if (component is null)
                    {
                        continue;
                    }
                    component.OnRouteChanged(e);
                }
            }
        }
        catch (RoutingException ex)
        {
            Trace.TraceError("Unable to navigate to path '{0}': {1}", path, ex);
        }
    }

    const string RootPath = "/";

    private IEnumerable<RouteChangedEvent> GetEventsForPath(string path)
    {
        var routeChangedEvents = Parse(path);
        if (routeChangedEvents.Count == 1)
        {
            yield return routeChangedEvents[0];
        }

        RouteChangedEvent currentEvent;
        RouteChangedEvent? nextEvent = null;

        for (int i = 0; i < routeChangedEvents.Count - 1; i++)
        {
            currentEvent = routeChangedEvents[i]!;
            nextEvent = routeChangedEvents[i + 1]!;
            yield return currentEvent with
            {
                NextChildNode = nextEvent.CurrentNode,
                NextChildParameters = nextEvent.CurrentParameters
            };
        }
        yield return nextEvent!;
    }

    private List<RouteChangedEvent> Parse(string path)
    {
        if (path?.StartsWith(RootPath) != true)
        {
            throw new ArgumentException($"Path must to start with '{RootPath}'", nameof(path));
        }

        var parsed = RoutePathParser.Parse(path);

        List<RouteChangedEvent> result = new();
        RouteChangedEvent routeChangedEvent = new RouteChangedEvent
        {
            CurrentNode = _root,
            NewPath = path,
            SegmentIndex = 0,
            CurrentParameters = parsed.Dequeue().Parameters

        };
        result.Add(routeChangedEvent);

        var currentNode = _root;
        var i = 0;

        while (parsed.TryDequeue(out var routeSegment))
        {
            i++;
            string? segment = routeSegment?.SegmentName;
            if (string.IsNullOrEmpty(segment))
            {
                throw new InvalidRouteException($"Detected empty segment (at index {i}) in the path: '{path}'");
            }

            //find route definition correspond to this segment
            if (!currentNode.HasChild(segment!))
            {
                throw new RouteNotFoundException($"Route's definition not found for the Segment '{segment}' (at index {i}) in the path: '{path}'");
            }

            result.Add(routeChangedEvent with
            {
                CurrentNode = currentNode[segment!],
                SegmentIndex = i,
                CurrentParameters = routeSegment.Parameters
            });
        }
        return result;
    }
}
