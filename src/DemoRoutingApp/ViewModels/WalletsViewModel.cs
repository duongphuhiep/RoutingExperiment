using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.BusinessLogic;
using DemoRoutingApp.Models;
using System;
using System.Threading.Tasks;

namespace DemoRoutingApp.ViewModels;

public partial class WalletsViewModel : ViewModelBase, IRoutable
{
    private readonly IWalletRepository? _walletRepository;

    [ObservableProperty]
    private RouteData _routeData;

    public Task<Wallet[]>? Wallets => LoadWallets();

    [ObservableProperty]
    private bool _isLoading;

    public WalletsViewModel(IWalletRepository? walletRepository)
    {
        _walletRepository = walletRepository;
    }

    protected virtual async Task<Wallet[]> LoadWallets()
    {
        ArgumentNullException.ThrowIfNull(_walletRepository);
        IsLoading = true;
        try
        {
            return await _walletRepository.GetWallets();
        }
        finally
        {
            IsLoading = false;
        }
    }
}

public class WalletsViewModelForDesigner : WalletsViewModel
{
    public WalletsViewModelForDesigner() : base(null)
    {
    }

    protected override async Task<Wallet[]> LoadWallets()
    {
        await Task.Yield();
        return [
            new Wallet { Id = 1, Name = "Wallet Designer 1", Balance = 100 },
            new Wallet { Id = 2, Name = "Wallet Designer 2", Balance = 200 }];
    }
}
