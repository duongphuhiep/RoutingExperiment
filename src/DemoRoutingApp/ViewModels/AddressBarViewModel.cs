using CommunityToolkit.Mvvm.ComponentModel;
using System.Web;

namespace DemoRoutingApp.ViewModels;

public partial class AddressBarViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _currentRoute;

    public AddressBarViewModel()
    {
        HttpUtility.ParseQueryString("/a/bc");
    }
}

public class AddressBarViewModelForDesigner : AddressBarViewModel
{
    public AddressBarViewModelForDesigner() : base()
    {
        CurrentRoute = "/a/b/c";
    }
}