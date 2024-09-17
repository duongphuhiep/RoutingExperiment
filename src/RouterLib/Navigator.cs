using System.Collections.Generic;

namespace Starfruit.RouterLib;

public interface INavigator
{
    string Goto(string targetAbsoluteAddress);
    string Goto(IRoutableViewModel fromViewModel, string targetRelativeAddress);
}

public class Navigator : INavigator
{
    private readonly RouteNodeDefinition _root;

    public Navigator(RouteNodeDefinition root)
    {
        _root = root;
    }

    public string Goto(string targetAbsoluteAddress)
    {
        var parsedPath = RoutePathParser.Parse(targetAbsoluteAddress);
        Goto(parsedPath);
        return targetAbsoluteAddress;
    }

    private void Goto(QueueStack<RouteSegment> parsedPath)
    {
        foreach (var e in GetEventsWithCurrentNodeAndNextChildNode(parsedPath))
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

    public string Goto(IRoutableViewModel fromViewModel, string targetRelativeAddress)
    {
        var baseAddress = fromViewModel.GetRouteAddress();
        var absolutePath = RoutePathParser.Combine(baseAddress, targetRelativeAddress);
        Goto(absolutePath);
        return absolutePath.ToStringAddress();
    }

    private IEnumerable<RouteChangedEvent> GetEventsWithCurrentNodeAndNextChildNode(QueueStack<RouteSegment> parsedPath)
    {
        var routeChangedEvents = GetEventsWithCurrentNodeOnly(parsedPath);
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

    private List<RouteChangedEvent> GetEventsWithCurrentNodeOnly(QueueStack<RouteSegment> parsedPath)
    {
        List<RouteChangedEvent> result = new();
        RouteChangedEvent routeChangedEvent = new RouteChangedEvent
        {
            CurrentNode = _root,
            SegmentIndex = 0,
            CurrentParameters = parsedPath.Dequeue().Parameters

        };
        result.Add(routeChangedEvent);

        var currentNode = _root;
        var i = 0;

        while (parsedPath.TryDequeue(out var routeSegment))
        {
            i++;
            string? segment = routeSegment?.SegmentName;
            if (string.IsNullOrEmpty(segment))
            {
                throw new InvalidRouteException($"Detected empty segment (at index {i}) in the path");
            }

            //find route definition correspond to this segment
            if (!currentNode.HasChild(segment!))
            {
                throw new RouteNotFoundException($"Route's definition not found for the Segment '{segment}' (at index {i}) in the path");
            }

            result.Add(routeChangedEvent with
            {
                CurrentNode = currentNode[segment!],
                SegmentIndex = i,
                CurrentParameters = routeSegment!.Parameters
            });
        }
        return result;
    }
}
