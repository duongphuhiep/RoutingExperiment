using System.Collections.Specialized;

namespace DemoRoutingApp.Models;

public record RouteChangedEvent
{
    public RouteNodeDefinition CurrentNode { get; init; } = new();
    public RouteNodeDefinition? NextChildNode { get; init; }
    public string NewPath { get; init; } = string.Empty;
    public NameValueCollection? QueryString { get; set; }
    public int SegmentIndex { get; init; }
}
