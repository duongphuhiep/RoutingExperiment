namespace Starfruit.RouterLib;

public record RouteSegmentChangedEvent
{
    public RouteNodeDefinition CurrentNode { get; internal set; } = new();
    public UnorderedKeyValueCollection? CurrentParameters { get; internal set; }
    public RouteNodeDefinition? NextChildNode { get; internal set; }
    public UnorderedKeyValueCollection? NextChildParameters { get; internal set; }
    public int SegmentIndex { get; internal set; }
}
