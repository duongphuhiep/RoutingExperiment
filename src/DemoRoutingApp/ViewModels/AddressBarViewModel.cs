using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Starfruit.RouterLib;
using System.Web;

namespace DemoRoutingApp.ViewModels;

public partial class AddressBarViewModel : ViewModelBase
{
    private readonly INavigator _navigator;
    [ObservableProperty]
    private string _currentRoute;

    public AddressBarViewModel(INavigator navigator)
    {
        HttpUtility.ParseQueryString("/a/bc");
        _navigator = navigator;
    }

    [RelayCommand]
    public void GotoRoute(string route)
    {
        _navigator.Goto(CurrentRoute);
    }
}

public class AddressBarViewModelForDesigner : AddressBarViewModel
{
    public AddressBarViewModelForDesigner() : base(null)
    {
        CurrentRoute = "/a/b/c";
    }
}