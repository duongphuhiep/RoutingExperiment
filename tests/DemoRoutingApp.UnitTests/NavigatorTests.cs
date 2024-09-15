using DemoRoutingApp.BusinessLogic;
using DemoRoutingApp.Models;
using DemoRoutingApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;

namespace DemoRoutingApp.UnitTests;

public class NavigatorTests
{
    [Fact]
    public void GotoTest()
    {
        var mainViewModel = App.ServiceProvider.GetRequiredService<MainViewModel>();
        var navigator = App.ServiceProvider.GetRequiredService<INavigator>();

        navigator.Goto("/personalWallets");
        mainViewModel.SelectedTabValue.Content.ShouldBeOfType<WalletsViewModel>();
        (mainViewModel.SelectedTabValue.Content as WalletsViewModel).WalletType.ShouldBe(BusinessLogic.WalletType.Personal);

        navigator.Goto("/companyWallets");
        mainViewModel.SelectedTabValue.Content.ShouldBeOfType<WalletsViewModel>();
        (mainViewModel.SelectedTabValue.Content as WalletsViewModel).WalletType.ShouldBe(BusinessLogic.WalletType.Company);

        navigator.Goto("/transfers");
        mainViewModel.SelectedTabValue.Content.ShouldBeOfType<TransfersViewModel>();
    }

    [Fact]
    public void WalletsViewModelTest()
    {
        IWalletRepository mockRepository = Substitute.For<IWalletRepository>();
        ServiceCollection services = App.BuildDependencyGraph();
        services.AddSingleton(mockRepository); //inject a fake repository instead of real repository which access to the database
        IServiceProvider serviceProviderWithMockRepository = services.BuildServiceProvider();

        WalletsViewModel subjectUnderTest = serviceProviderWithMockRepository.GetRequiredService<WalletsViewModel>();
        subjectUnderTest.Wallets.ShouldNotBeNull();
    }
}
