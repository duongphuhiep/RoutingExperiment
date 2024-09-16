namespace Starfruit.RouterLib;

public class InvalidRouteException : RoutingException
{
    public InvalidRouteException(string message) : base(message) { }
}