using Shouldly;
using Starfruit.RouterLib;
using Xunit.Abstractions;

namespace DemoRoutingApp.UnitTests;

public class RoutePathParserTests
{
    private readonly ITestOutputHelper _output;

    public RoutePathParserTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void SegmentOnly()
    {
        string route = "/aa/bb";
        var result = RoutePathParser.Parse(route);
        result.Count.ShouldBe(3);
        var segment0 = result.Dequeue(); //root
        segment0.Segment.ShouldBeEmpty();
        segment0.Parameters.Count.ShouldBe(0);
        var segment1 = result.Dequeue();
        segment1.Segment.ShouldBe("aa");
        segment1.Parameters.Count.ShouldBe(0);
        var segment2 = result.Dequeue();
        segment2.Segment.ShouldBe("bb");
        segment2.Parameters.Count.ShouldBe(0);
    }
    [Fact]
    public void SimpleParameters()
    {
        string route = "/aa:k1=v1:k2=v2/bb:k3=v3";
        var result = RoutePathParser.Parse(route);
        result.Count.ShouldBe(3);
        var segment0 = result.Dequeue(); //root
        segment0.Segment.ShouldBeEmpty();
        segment0.Parameters.Count.ShouldBe(0);
        var segment1 = result.Dequeue();
        segment1.Segment.ShouldBe("aa");
        segment1.Parameters.Count.ShouldBe(2);
        segment1.Parameters["k1"].ShouldBe("v1");
        segment1.Parameters["k2"].ShouldBe("v2");
        var segment2 = result.Dequeue();
        segment2.Segment.ShouldBe("bb");
        segment2.Parameters.Count.ShouldBe(1);
        segment2.Parameters["k3"].ShouldBe("v3");
    }
    [Fact]
    public void EmptyParametersValues()
    {
        string route = "/aa:k1=:k2=v2/bb:k3=/cc";
        var result = RoutePathParser.Parse(route);
        result.Count.ShouldBe(4);
        var segment0 = result.Dequeue(); //root
        segment0.Segment.ShouldBeEmpty();
        segment0.Parameters.Count.ShouldBe(0);
        var segment1 = result.Dequeue();
        segment1.Segment.ShouldBe("aa");
        segment1.Parameters.Count.ShouldBe(2);
        segment1.Parameters["k1"].ShouldBeEmpty();
        segment1.Parameters["k2"].ShouldBe("v2");
        var segment2 = result.Dequeue();
        segment2.Segment.ShouldBe("bb");
        segment2.Parameters.Count.ShouldBe(1);
        segment2.Parameters["k3"].ShouldBeEmpty();
        var segment3 = result.Dequeue();
        segment3.Segment.ShouldBe("cc");
        segment3.Parameters.Count.ShouldBe(0);
    }
    [Fact]
    public void EmptyOrSameParametersKeys()
    {
        string route = "/aa:=v1:=v2/bb:k1=v3:k1=v4";
        var result = RoutePathParser.Parse(route);
        result.Count.ShouldBe(3);
        var segment0 = result.Dequeue(); //root
        segment0.Segment.ShouldBeEmpty();
        segment0.Parameters.Count.ShouldBe(0);
        var segment1 = result.Dequeue();
        segment1.Segment.ShouldBe("aa");
        segment1.Parameters.Count.ShouldBe(1);
        segment1.Parameters[""].ShouldBe("v1,v2");
        var segment2 = result.Dequeue();
        segment2.Segment.ShouldBe("bb");
        segment2.Parameters["k1"].ShouldBe("v3,v4");
    }

    [Fact]
    public void EscapeCharacter()
    {
        string route = "/aa:k\\:1=v1:k2=\\=v2/b\\/b:k3=v3";
        var result = RoutePathParser.Parse(route);
        result.Count.ShouldBe(3);
        var segment0 = result.Dequeue(); //root
        segment0.Segment.ShouldBeEmpty();
        segment0.Parameters.Count.ShouldBe(0);
        var segment1 = result.Dequeue();
        segment1.Segment.ShouldBe("aa");
        segment1.Parameters.Count.ShouldBe(2);
        segment1.Parameters["k:1"].ShouldBe("v1");
        segment1.Parameters["k2"].ShouldBe("=v2");
        var segment2 = result.Dequeue();
        segment2.Segment.ShouldBe("b/b");
        segment2.Parameters.Count.ShouldBe(1);
        segment2.Parameters["k3"].ShouldBe("v3");
    }
    [Theory()]
    [InlineData("Segment not assignable", "/aa/bb=1")]
    [InlineData("Missing key", "/aa:=v1=v2/bb")]
    [InlineData("Missing key", "/aa:k1=v1=v2/bb")]
    [InlineData("Missing value", "/aa:k1:k2=/bb")]
    [InlineData("Missing value", "/aa:k1/bb")]
    public void InvalidRoutePath(string description, string route)
    {
        var ex = Assert.Throws<InvalidRouteException>(() => RoutePathParser.Parse(route));
        _output.WriteLine($"{description}{Environment.NewLine}{route}{Environment.NewLine}{ex.Message}{Environment.NewLine} --");
    }
}
