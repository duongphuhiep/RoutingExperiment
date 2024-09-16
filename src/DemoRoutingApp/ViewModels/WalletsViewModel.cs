using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.BusinessLogic;
using Starfruit.RouterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoRoutingApp.ViewModels;

public partial class WalletsViewModel : RoutableViewModel
{
    private readonly IWalletRepository? _walletRepository;

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

    public WalletsViewModel(IWalletRepository? walletRepository, WalletDetailViewModel walletDetailViewModel)
    {
        _walletRepository = walletRepository;
        WalletDetailViewModel = walletDetailViewModel;
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

    partial void OnSelectedWalletChanged(Wallet value)
    {

    }

    public override void RegisterChildren()
    {
        this.RegisterChild("walletDetails", WalletDetailViewModel);
    }

    public override void OnRouteChanged(RouteChangedEvent e)
    {
    }
}

public class WalletsViewModelForDesigner : WalletsViewModel
{
    public WalletsViewModelForDesigner() : base(null, new WalletDetailViewModelForDesigner())
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
