using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Starfruit.RouterLib;

namespace DemoRoutingApp.UnitTests;

public class DiTests
{
    [Fact]
    public void UnorderedKeyValueCollectionTest()
    {
        UnorderedKeyValueCollection a = new UnorderedKeyValueCollection();
        UnorderedKeyValueCollection b = new UnorderedKeyValueCollection();
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
        a.Add("k3", "v3");
        a.Add("k3", "v3bis");
        EqualityComparer<UnorderedKeyValueCollection>.Default.Equals(a, b).ShouldBeTrue();
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
