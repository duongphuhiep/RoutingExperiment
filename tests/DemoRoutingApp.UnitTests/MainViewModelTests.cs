using DemoRoutingApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DemoRoutingApp.UnitTests;

public class MainViewModelTests
{
    private readonly MainViewModel _sut;

    public MainViewModelTests()
    {
        _sut = App.ServiceProvider.GetRequiredService<MainViewModel>();
    }

    [Fact]
    public void ChangeRoutingTest()
    {

    }
}
