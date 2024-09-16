using System.Collections.Specialized;

namespace Starfruit.RouterLib;

public record RouteChangedEvent
{
    public RouteNodeDefinition CurrentNode { get; internal set; } = new();
    public NameValueCollection CurrentParameters { get; internal set; } = new();
    public RouteNodeDefinition? NextChildNode { get; internal set; }
    public NameValueCollection NextChildParameters { get; internal set; } = new();
    public string NewPath { get; internal set; } = string.Empty;
    public int SegmentIndex { get; internal set; }
}
