using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace DemoRoutingApp.ViewModels;

public partial class MainViewModel : RoutableViewModel
{
    public ObservableCollection<TabItemViewModel> Tabs { get; set; }

    [ObservableProperty]
    private TabItemViewModel? _selectedTabValue;

    public MainViewModel(WalletsViewModel walletsViewModel, TransfersViewModel transfersViewModel)
    {
        Tabs = new ObservableCollection<TabItemViewModel>
        {
            new TabItemViewModel{ Content = walletsViewModel, Header = "Wallets" },
            new TabItemViewModel{ Content = transfersViewModel, Header = "Transfers" }
        };
    }

    protected override void OnRouteSelectedChildChanged()
    {
        SelectedTabValue = Tabs.First(t => t.Content?.GetType() == RouteData.SelectedChild?.Component);
    }
}

public class MainViewModelForDesigner : MainViewModel
{
    public MainViewModelForDesigner() : base(new WalletsViewModelForDesigner(), new TransfersViewModelForDesigner())
    {
    }
}
