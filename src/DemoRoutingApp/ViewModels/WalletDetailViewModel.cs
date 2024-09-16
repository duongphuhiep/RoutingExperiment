﻿using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.BusinessLogic;
using Starfruit.RouterLib;
using System;
using System.Threading.Tasks;

namespace DemoRoutingApp.ViewModels;

public partial class WalletDetailViewModel : RoutableViewModel
{
    private readonly IWalletRepository? _walletRepository;


    [ObservableProperty]
    private bool _isLoading;

    public Task<Wallet?> Wallet => LoadWallet();

    public WalletDetailViewModel(IWalletRepository? walletRepository)
    {
        _walletRepository = walletRepository;
    }

    protected virtual async Task<Wallet?> LoadWallet()
    {
        ArgumentNullException.ThrowIfNull(_walletRepository);
        if (RouteSegmentParameters is null)
        {
            return null;
        }
        var walletId = RouteSegmentParameters["walletId"];
        if (string.IsNullOrEmpty(walletId))
        {
            return null;
        }
        IsLoading = true;
        try
        {
            return await _walletRepository.GetWallet(1); //TODO
        }
        finally
        {
            IsLoading = false;
        }
    }

    public override void RegisterChildren()
    {
    }

    public override void OnRouteChanged(RouteChangedEvent e)
    {
    }
}

public class WalletDetailViewModelForDesigner : WalletDetailViewModel
{
    public WalletDetailViewModelForDesigner() : base(null)
    {
    }
    protected override async Task<Wallet?> LoadWallet()
    {
        await Task.Yield();
        return new Wallet { Id = 1, Name = "Wallet Designer", Balance = 100 };
    }
}

