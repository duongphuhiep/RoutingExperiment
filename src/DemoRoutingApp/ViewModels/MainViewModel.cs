using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.Models;
using Starfruit.RouterLib;
using System.Collections.ObjectModel;
using System.Linq;

namespace DemoRoutingApp.ViewModels;

public partial class MainViewModel : RoutableViewModel
{
    public ObservableCollection<TabItemViewModel> Tabs { get; set; }

    [ObservableProperty]
    private TabItemViewModel? _selectedTabValue;

    private readonly WalletsViewModel _personalWalletsViewModel;
    private readonly WalletsViewModel _companyWalletsViewModel;
    private readonly TransfersViewModel _transfersViewModel;

    public MainViewModel(WalletsViewModel personalWalletsViewModel, WalletsViewModel companyWalletsViewModel, TransfersViewModel transfersViewModel)
    {
        _personalWalletsViewModel = personalWalletsViewModel;
        _personalWalletsViewModel.WalletType = BusinessLogic.WalletType.Personal;

        _companyWalletsViewModel = companyWalletsViewModel;
        _companyWalletsViewModel.WalletType = BusinessLogic.WalletType.Company;

        _transfersViewModel = transfersViewModel;
        Tabs = new ObservableCollection<TabItemViewModel>
        {
            new() { Content = personalWalletsViewModel, Header = "Personal Wallets", Name="personalWallets" },
            new() { Content = companyWalletsViewModel, Header = "Company Wallets", Name="companyWallets" },
            new() { Content = transfersViewModel, Header = "Transfers", Name="transfers" }
        };
        this.RegisterAsRoot(RouterConfig.Root);
    }

    public override void RegisterChildren()
    {
        this.RegisterChild("personalWallets", _personalWalletsViewModel);
        this.RegisterChild("companyWallets", _companyWalletsViewModel);
        this.RegisterChild("transfers", _transfersViewModel);
    }

    public override void OnRouteChanged(RouteChangedEvent e)
    {
        SelectedTabValue = Tabs.First(t => e.NextChildNode!.SegmentName == t.Name);
    }
}

public class MainViewModelForDesigner : MainViewModel
{
    public MainViewModelForDesigner() : base(new WalletsViewModelForDesigner(), new WalletsViewModelForDesigner(), new TransfersViewModelForDesigner())
    {
    }
}
