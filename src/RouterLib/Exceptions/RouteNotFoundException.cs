namespace Starfruit.RouterLib;

public class RouteNotFoundException : RoutingException
{
    public RouteNotFoundException(string message) : base(message) { }
}