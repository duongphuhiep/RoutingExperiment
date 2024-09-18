using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Starfruit.RouterLib;

namespace DemoRoutingApp.UnitTests;

public class DiTests
{
    [Fact]
    public void UnorderedKeyValueCollection_EqualsTest()
    {
        UnorderedKeyValueCollection a = new UnorderedKeyValueCollection();
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(null, a).ShouldBeFalse();
        UnorderedKeyValueCollection b = new UnorderedKeyValueCollection();
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeTrue();
        a.Add("k1", "v1");
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeFalse();
        b.Add("k1", "v1");
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeTrue();

        a.Add("k2", "v2");
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeFalse();
        b.Add("k2", "v2bis");
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeFalse();
        b["k2"] = "v2";
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeTrue();

        b.Add("k3", "v3");
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeFalse();
        b.Add("k3", "v3bis");
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeFalse();
        a.Add("k3", "v3bis");
        a.Add("k3", "v3");
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeTrue();
    }

    [Fact]
    public void RouteSegment_EqualsTest()
    {
        RouteSegment a1 = new()
        {
            SegmentName = "a",
            Parameters = new UnorderedKeyValueCollection
            {
                { "k1", "v1" },
                { "k2", "v2" }
            }
        };
        RouteSegment a2 = new()
        {
            SegmentName = "a",
            Parameters = new UnorderedKeyValueCollection
            {
                { "k2", "v2" },
                { "k1", "v1" }
            }
        };
        EqualityComparer<RouteSegment>.Default.Equals(a1, a2).ShouldBeTrue();

        RouteSegment a3 = new()
        {
            SegmentName = "aa",
            Parameters = new UnorderedKeyValueCollection
            {
                { "k2", "v2" },
                { "k1", "v1" }
            }
        };
        EqualityComparer<RouteSegment>.Default.Equals(a1, a3).ShouldBeFalse();
        RouteSegment a4 = new()
        {
            SegmentName = "a",
            Parameters = new UnorderedKeyValueCollection
            {
                { "k1", "v1bis" },
                { "k2", "v2" }
            }
        };
        EqualityComparer<RouteSegment>.Default.Equals(a1, a4).ShouldBeFalse();
    }

    [Fact]
    public void TransientTest()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddTransient<A>();
        services.AddTransient<B>();
        var provider = services.BuildServiceProvider();
        var b = provider.GetRequiredService<B>();
        b.A1.ShouldNotBe(b.A2);
    }

    [Fact]
    public void SingletonTest()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<A>();
        services.AddSingleton<B>();
        var provider = services.BuildServiceProvider();
        var b = provider.GetRequiredService<B>();
        b.A1.ShouldBe(b.A2);
    }
    [Fact]
    public void MixedTest()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddTransient<A>();
        services.AddSingleton<A>();
        services.AddSingleton<B>();
        var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        var b = provider.GetRequiredService<B>();
        b.A1.ShouldBe(b.A2);
    }

    [Fact]
    public void AssignableTest()
    {
        typeof(A).IsAssignableFrom(typeof(A1)).ShouldBeTrue();
        typeof(A1).IsAssignableTo(typeof(A)).ShouldBeTrue();
    }

    interface IComp;

    class Node<T> where T : IComp
    {
        public static explicit operator Node<T>(Node<A> v)
        {
            throw new NotImplementedException();
        }
    }

    class A : IComp;
    class B(A a1, A a2) : IComp
    {
        public A A1 => a1;
        public A A2 => a2;
    };
    class A1 : A;
}
