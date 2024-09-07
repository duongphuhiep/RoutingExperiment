using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace DemoRoutingApp.ViewModels;

public partial class MainViewModel : RoutableViewModel
{
    public ObservableCollection<TabItemViewModel> Tabs { get; set; }

    [ObservableProperty]
    private TabItemViewModel? _selectedTabValue;
    private readonly WalletsViewModel _walletsViewModel;
    private readonly TransfersViewModel _transfersViewModel;

    public MainViewModel(WalletsViewModel walletsViewModel, TransfersViewModel transfersViewModel)
    {
        this._walletsViewModel = walletsViewModel;
        this._transfersViewModel = transfersViewModel;
        Tabs = new ObservableCollection<TabItemViewModel>
        {
            new() { Content = walletsViewModel, Header = "Wallets" },
            new() { Content = transfersViewModel, Header = "Transfers" }
        };
        AttachToRouteDefinition(RouterConfig.Root);
    }

    public override void AttachChildrenToRouteDefinitions()
    {
        _walletsViewModel.AttachToRouteDefinition(RouteDefinition!["wallets"]);
        _transfersViewModel.AttachToRouteDefinition(RouteDefinition["transfers"]);
    }

    public override void OnRouteChanged(RouteChangedEvent e)
    {
        SelectedTabValue = Tabs.First(t => e.NextChildNode!.ComponentType.IsInstanceOfType(t.Content));
    }
}

public class MainViewModelForDesigner : MainViewModel
{
    public MainViewModelForDesigner() : base(new WalletsViewModelForDesigner(), new TransfersViewModelForDesigner())
    {
    }
}
