using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;

namespace Starfruit.RouterLib;

public interface INavigator
{
    string Goto(string targetAbsoluteAddress);
    string Goto(IRoutableViewModel fromViewModel, string targetRelativeAddress);
}

public partial class Navigator : ObservableRecipient, INavigator
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
        Messenger.Send(new RouteChangingEvent { NewRouteSegments = parsedPath });
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
                component.RouteSegmentParameters = e.CurrentParameters;
                component.OnRouteChanged(e);
            }
        }
        Messenger.Send(new RouteChangedEvent { NewRouteSegments = parsedPath });
    }

    public string Goto(IRoutableViewModel fromViewModel, string targetRelativeAddress)
    {
        var baseAddress = fromViewModel.GetRouteAddress();
        var absolutePath = RoutePathParser.Combine(baseAddress, targetRelativeAddress);
        Goto(absolutePath);
        return absolutePath.ToStringAddress();
    }

    private IEnumerable<RouteSegmentChangedEvent> GetEventsWithCurrentNodeAndNextChildNode(QueueStack<RouteSegment> parsedPath)
    {
        var routeChangedEvents = GetEventsWithCurrentNodeOnly(parsedPath);
        if (routeChangedEvents.Count == 1)
        {
            yield return routeChangedEvents[0];
        }

        RouteSegmentChangedEvent currentEvent;
        RouteSegmentChangedEvent? nextEvent = null;

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

    private List<RouteSegmentChangedEvent> GetEventsWithCurrentNodeOnly(QueueStack<RouteSegment> parsedPath)
    {
        List<RouteSegmentChangedEvent> result = new();
        RouteSegmentChangedEvent routeChangedEvent = new RouteSegmentChangedEvent
        {
            CurrentNode = _root,
            SegmentIndex = 0,
            CurrentParameters = parsedPath.Dequeue().Parameters

        };
        result.Add(routeChangedEvent);

        var currentNode = _root;
        var i = 0;

        foreach (var routeSegment in parsedPath)
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
            currentNode = currentNode[segment!];

            result.Add(routeChangedEvent with
            {
                CurrentNode = currentNode,
                SegmentIndex = i,
                CurrentParameters = routeSegment!.Parameters
            });
        }
        return result;
    }
}
