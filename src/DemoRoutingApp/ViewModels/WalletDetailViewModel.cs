using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.BusinessLogic;
using System;
using System.Threading.Tasks;

namespace DemoRoutingApp.ViewModels;

public partial class WalletDetailViewModel : ViewModelBase
{
    private readonly IWalletRepository? _walletRepository;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Wallet))]
    private int? _walletId;

    [ObservableProperty]
    private bool _isLoading;

    public Task<Wallet?> Wallet => WalletId.HasValue ? LoadWallet() : Task.FromResult<Wallet?>(null);

    public WalletDetailViewModel(IWalletRepository? walletRepository)
    {
        _walletRepository = walletRepository;
    }

    protected virtual async Task<Wallet?> LoadWallet()
    {
        ArgumentNullException.ThrowIfNull(_walletRepository);
        if (!WalletId.HasValue)
        {
            return null;
        }
        IsLoading = true;
        try
        {
            return await _walletRepository.GetWallet(WalletId.Value);
        }
        finally
        {
            IsLoading = false;
        }
    }
}

public class WalletDetailViewModelForDesigner : WalletDetailViewModel
{
    public WalletDetailViewModelForDesigner(IWalletRepository walletRepository) : base(null)
    {
    }
    protected override async Task<Wallet?> LoadWallet()
    {
        await Task.Yield();
        return new Wallet { Id = 1, Name = "Wallet Designer", Balance = 100 };
    }
}

