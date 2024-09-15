using System.Collections.Specialized;

namespace DemoRoutingApp.Models;

public record RouteChangedEvent
{
    public RouteNodeDefinition CurrentNode { get; init; } = new();
    public NameValueCollection CurrentParameters { get; init; } = new();
    public RouteNodeDefinition? NextChildNode { get; init; }
    public NameValueCollection NextChildParameters { get; init; } = new();
    public string NewPath { get; init; } = string.Empty;
    public int SegmentIndex { get; init; }
}
