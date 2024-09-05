using Shouldly;
using System.Collections.Specialized;
using System.Web;
using Xunit.Abstractions;

namespace DemoRoutingApp.UnitTests;

public class RouteParserTests
{
    private readonly ITestOutputHelper _output;

    public RouteParserTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void UriTest()
    {
        {
            var uri = new Uri("route:a/b/c/");
            uri.Segments.ShouldBe(["a/", "b/", "c/"]);
            NameValueCollection queryString = HttpUtility.ParseQueryString(uri.Query);
            queryString.ShouldBe([]);
        }
        {
            var uri = new Uri("route:/a/b/c/../d?k=v&k2=v2");
            uri.Segments.ShouldBe(["/", "a/", "b/", "c/", "../", "d"]);
            uri.Query.ShouldBe("?k=v&k2=v2");

            NameValueCollection queryString = HttpUtility.ParseQueryString(uri.Query);
            queryString["k"].ShouldBe("v");
            queryString["k2"].ShouldBe("v2");
        }
    }

    [Fact]
    public void InheritanceTest()
    {
        var b = new B();
    }

    class A
    {
        public A()
        {
            Console.WriteLine("A");
        }
    }
    class B : A
    {
        public B()
        {
            Console.WriteLine("B");
        }
    }
}