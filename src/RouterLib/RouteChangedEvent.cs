﻿namespace Starfruit.RouterLib;

public record RouteChangedEvent
{
    /// <summary>
    /// Use <seealso cref="RoutePathParser.ToStringAddress(QueueStack{RouteSegment})"/> to convert this value to string.
    /// </summary>
    public QueueStack<RouteSegment> NewRouteSegments { get; internal set; }
}
