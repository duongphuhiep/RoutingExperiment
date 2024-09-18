using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Starfruit.RouterLib;

namespace DemoRoutingApp.ViewModels;

public partial class AddressBarViewModel : ViewModelBase, IRecipient<RouteChangedEvent>
{
    private readonly INavigator _navigator;
    [ObservableProperty]
    private string _currentRoute;

    public AddressBarViewModel(INavigator navigator)
    {
        _navigator = navigator;
        IsActive = true;
    }

    [RelayCommand]
    public void GotoRoute(string route)
    {
        _navigator.Goto(CurrentRoute);
    }

    public void Receive(RouteChangedEvent message)
    {
        CurrentRoute = message.NewRouteSegments.ToStringAddress();
    }
}

public class AddressBarViewModelForDesigner : AddressBarViewModel
{
    public AddressBarViewModelForDesigner() : base(null)
    {
        CurrentRoute = "/a/b/c";
    }
}