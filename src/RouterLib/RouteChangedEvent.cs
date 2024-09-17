namespace Starfruit.RouterLib;

public record RouteChangedEvent
{
    public RouteNodeDefinition CurrentNode { get; internal set; } = new();
    public UnorderedKeyValueCollection CurrentParameters { get; internal set; } = new();
    public RouteNodeDefinition? NextChildNode { get; internal set; }
    public UnorderedKeyValueCollection NextChildParameters { get; internal set; } = new();
    public int SegmentIndex { get; internal set; }
}
