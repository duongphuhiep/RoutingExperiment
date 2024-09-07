namespace DemoRoutingApp.Models;

public class InvalidRouteException : RoutingException
{
    public InvalidRouteException(string message) : base(message) { }
}
