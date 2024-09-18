namespace Starfruit.RouterLib;

public record RouteSegment
{
    public string? SegmentName { get; set; }
    public UnorderedKeyValueCollection? Parameters { get; set; } = new();
}
