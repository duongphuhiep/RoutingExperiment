using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.BusinessLogic;
using DemoRoutingApp.Models;
using Starfruit.RouterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoRoutingApp.ViewModels;

public partial class WalletsViewModel : RoutableViewModel
{
    private readonly IWalletRepository? _walletRepository;
    private readonly INavigator _navigator;
    [ObservableProperty]
    private WalletDetailViewModel _walletDetailViewModel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Wallets))]
    private WalletType? _walletType;

    public Task<IEnumerable<Wallet>> Wallets => LoadWallets(WalletType);

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private Wallet? _selectedWallet;

    public WalletsViewModel(IWalletRepository? walletRepository, WalletDetailViewModel walletDetailViewModel, INavigator navigator)
    {
        _walletRepository = walletRepository;
        WalletDetailViewModel = walletDetailViewModel;
        _navigator = navigator;
    }

    protected virtual async Task<IEnumerable<Wallet>> LoadWallets(WalletType? walletType)
    {
        ArgumentNullException.ThrowIfNull(_walletRepository);
        if (!walletType.HasValue)
        {
            return [];
        }
        IsLoading = true;
        try
        {
            return (await _walletRepository.GetWallets(walletType.Value)).AsEnumerable();
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnSelectedWalletChanged(Wallet? value)
    {
        if (value is null) { return; }
        _navigator.Goto(this, $"walletDetails:walletId={value.Id}");
    }

    public override void RegisterChildren()
    {
        this.RegisterChild("walletDetails", WalletDetailViewModel);
    }

    public override void OnRouteChanged(RouteSegmentChangedEvent e)
    {
    }
}

public class WalletsViewModelForDesigner : WalletsViewModel
{
    public WalletsViewModelForDesigner() : base(null, new WalletDetailViewModelForDesigner(), new Navigator(RouterConfig.Root))
    {
    }

    protected override async Task<IEnumerable<Wallet>> LoadWallets(WalletType? walletType)
    {
        await Task.Yield();
        return [
            new Wallet { Id = 1, Name = "Wallet Designer 1", Balance = 100, Type=walletType ?? BusinessLogic.WalletType.Personal },
            new Wallet { Id = 2, Name = "Wallet Designer 2", Balance = 200, Type=walletType ?? BusinessLogic.WalletType.Company }];
    }
}
