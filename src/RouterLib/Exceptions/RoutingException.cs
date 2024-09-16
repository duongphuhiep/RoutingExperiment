using System;

namespace Starfruit.RouterLib;

public class RoutingException : Exception
{
    public RoutingException(string message) : base(message) { }
}
