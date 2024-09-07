using System;

namespace DemoRoutingApp.Models;

public class RoutingException : Exception
{
    public RoutingException(string message) : base(message) { }
}
