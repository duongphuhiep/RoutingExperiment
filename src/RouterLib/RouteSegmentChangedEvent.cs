namespace Starfruit.RouterLib;

public record BaseRouteSegmentChangeEvent
{
    public RouteNodeDefinition CurrentNode { get; internal set; } = new();
    public UnorderedKeyValueCollection? CurrentParameters { get; internal set; }
    public RouteNodeDefinition? NextChildNode { get; internal set; }
    public UnorderedKeyValueCollection? NextChildParameters { get; internal set; }
    public int SegmentIndex { get; internal set; }

    public BaseRouteSegmentChangeEvent()
    {

    }
    public BaseRouteSegmentChangeEvent(BaseRouteSegmentChangeEvent origin)
    {
        CurrentNode = origin.CurrentNode;
        CurrentParameters = origin.CurrentParameters;
        NextChildNode = origin.NextChildNode;
        NextChildParameters = origin.NextChildParameters;
        SegmentIndex = origin.SegmentIndex;
    }
}

public record RouteSegmentChangingEvent : BaseRouteSegmentChangeEvent
{
    public RouteSegmentChangingEvent()
    {

    }
    public RouteSegmentChangingEvent(BaseRouteSegmentChangeEvent origin) : base(origin)
    {

    }
}
public record RouteSegmentChangedEvent : BaseRouteSegmentChangeEvent
{
    public RouteSegmentChangedEvent() { }
    public RouteSegmentChangedEvent(BaseRouteSegmentChangeEvent origin) : base(origin) { }
}
