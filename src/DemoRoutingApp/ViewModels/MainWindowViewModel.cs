using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoRoutingApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private MainViewModel mainViewModel;

    public MainWindowViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
    }
}

public class MainWindowViewModelForDesigner : MainWindowViewModel
{
    public MainWindowViewModelForDesigner() : base(new MainViewModelForDesigner())
    {
    }
}
