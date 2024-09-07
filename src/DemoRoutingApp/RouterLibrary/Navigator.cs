using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;

namespace DemoRoutingApp.Models;

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
    const string Separator = "/";

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
                NextChildNode = nextEvent.CurrentNode
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

        Uri uri = new Uri("route:" + path);
        NameValueCollection queryString = HttpUtility.ParseQueryString(uri.Query);

        List<RouteChangedEvent> result = new();
        RouteChangedEvent routeChangedEvent = new RouteChangedEvent
        {
            CurrentNode = _root,
            NewPath = path,
            QueryString = queryString,
            SegmentIndex = 0,

        };
        result.Add(routeChangedEvent);

        var currentNode = _root;
        for (int i = 1; i < uri.Segments.Length; i++) // next node
        {
            string rawSegment = uri.Segments[i];
            var segment = CleanSegment(rawSegment);
            if (string.IsNullOrEmpty(segment))
            {
                throw new InvalidRouteException($"Detected empty segment (at index {i}) in the path: '{path}'");
            }

            //find route definition correspond to this segment
            if (!currentNode.HasChild(segment))
            {
                throw new RouteNotFoundException($"Route's definition not found for the Segment '{segment}' (at index {i}) in the path: '{path}'");
            }
            result.Add(routeChangedEvent with
            {
                CurrentNode = currentNode[segment],
                SegmentIndex = i,
            });
        }
        return result;
    }
    private static string CleanSegment(string rawSegment)
    {
        if (string.IsNullOrEmpty(rawSegment))
        {
            return string.Empty;
        }
        if (rawSegment == RootPath) { return RootPath; }
        var segment = rawSegment.Replace(Separator, string.Empty);
        return segment.Trim();
    }
}
