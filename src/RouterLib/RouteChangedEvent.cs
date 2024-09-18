namespace Starfruit.RouterLib;

public record BaseRouteChangeEvent
{
    /// <summary>
    /// Use <seealso cref="RoutePathParser.ToStringAddress(QueueStack{RouteSegment})"/> to convert this value to string.
    /// </summary>
    public QueueStack<RouteSegment> NewRouteSegments { get; internal set; } = new();
    public BaseRouteChangeEvent()
    {

    }
    public BaseRouteChangeEvent(BaseRouteChangeEvent origin)
    {
        NewRouteSegments = origin.NewRouteSegments;
    }
}

public record RouteChangedEvent : BaseRouteChangeEvent
{
    public RouteChangedEvent()
    {

    }
    public RouteChangedEvent(BaseRouteChangeEvent origin) : base(origin)
    {

    }
}
public record RouteChangingEvent : BaseRouteChangeEvent
{
    public RouteChangingEvent()
    {

    }
    public RouteChangingEvent(BaseRouteChangeEvent origin) : base(origin)
    {

    }
}