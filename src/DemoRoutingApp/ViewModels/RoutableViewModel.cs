using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.Models;
using System.ComponentModel;

namespace DemoRoutingApp.ViewModels;

public partial class RoutableViewModel : ViewModelBase, IRoutable
{
    [ObservableProperty]
    private RouteData routeData;

    public RoutableViewModel()
    {
        RouteData.PropertyChanged += RouteData_PropertyChanged;
    }

    partial void OnRouteDataChanged(RouteData? oldValue, RouteData newValue)
    {
        RouteData.PropertyChanged += RouteData_PropertyChanged;
    }

    private void RouteData_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(RouteData.SelectedChild))
        {
            return;
        }
        OnRouteSelectedChildChanged();
    }

    protected virtual void OnRouteSelectedChildChanged() { }

}
