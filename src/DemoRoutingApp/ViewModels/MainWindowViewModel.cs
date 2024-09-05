using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoRoutingApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private MainViewModel _mainViewModel;
    [ObservableProperty]
    private AddressBarViewModel _addressBarViewModel;

    public MainWindowViewModel(MainViewModel mainViewModel, AddressBarViewModel addressBarViewModel)
    {
        _mainViewModel = mainViewModel;
        _addressBarViewModel = addressBarViewModel;
    }
}

public class MainWindowViewModelForDesigner : MainWindowViewModel
{
    public MainWindowViewModelForDesigner() : base(new MainViewModelForDesigner(), new AddressBarViewModelForDesigner())
    {
    }
}
