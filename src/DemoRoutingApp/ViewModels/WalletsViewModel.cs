using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.BusinessLogic;
using DemoRoutingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoRoutingApp.ViewModels;

public partial class WalletsViewModel : RoutableViewModel
{
    private readonly IWalletRepository? _walletRepository;

    [ObservableProperty]
    private WalletType? _walletType;

    [ObservableProperty]
    public Task<IEnumerable<Wallet>> _wallets;

    [ObservableProperty]
    private bool _isLoading;

    public WalletsViewModel(IWalletRepository? walletRepository)
    {
        _walletRepository = walletRepository;
    }

    partial void OnWalletTypeChanged(WalletType? value)
    {
        _wallets = LoadWallets(value);
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

    public override void AttachChildrenToRouteDefinitions()
    {
    }

    public override void OnRouteChanged(RouteChangedEvent e)
    {
    }
}

public class WalletsViewModelForDesigner : WalletsViewModel
{
    public WalletsViewModelForDesigner() : base(null)
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
