using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.BusinessLogic;
using Starfruit.RouterLib;
using System;
using System.Threading.Tasks;

namespace DemoRoutingApp.ViewModels;

public partial class TransfersViewModel : RoutableViewModel
{
    private readonly ITransferRepository? _transferRepository;

    public Task<Transfer[]>? Transfers => LoadTransfers();

    [ObservableProperty]
    private bool _isLoading;

    public TransfersViewModel(ITransferRepository? transferRepository)
    {
        _transferRepository = transferRepository;
    }

    protected virtual async Task<Transfer[]> LoadTransfers()
    {
        ArgumentNullException.ThrowIfNull(_transferRepository);
        IsLoading = true;
        try
        {
            return await _transferRepository.GetTransfers();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public override void RegisterChildren()
    {
    }

    public override void OnRouteChanged(RouteSegmentChangedEvent e)
    {
    }
}

public class TransfersViewModelForDesigner : TransfersViewModel
{
    public TransfersViewModelForDesigner() : base(null)
    {
    }

    protected override async Task<Transfer[]> LoadTransfers()
    {
        await Task.Yield();
        return [
            new Transfer { Id = 1, WalletSender = 1, WalletReceiver = 2, Amount = 100 },
            new Transfer { Id = 2, WalletSender = 2, WalletReceiver = 1, Amount = 200 }];
    }
}
