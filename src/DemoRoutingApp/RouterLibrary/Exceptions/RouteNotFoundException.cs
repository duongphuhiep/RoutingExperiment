namespace DemoRoutingApp.Models;

public class RouteNotFoundException : RoutingException
{
    public RouteNotFoundException(string message) : base(message) { }
}