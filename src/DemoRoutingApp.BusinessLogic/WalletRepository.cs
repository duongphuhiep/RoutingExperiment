namespace DemoRoutingApp.BusinessLogic;

public interface IWalletRepository
{
    Task<Wallet?> GetWallet(int id);
    Task<Wallet[]> GetWallets();
    Task<Wallet[]> GetWallets(WalletType walletType);
}

internal class WalletRepository : IWalletRepository
{
    public async Task<Wallet?> GetWallet(int id)
    {
        await Task.Delay(100);
        return Database.Wallets.Find(x => x.Id == id);
    }

    public async Task<Wallet[]> GetWallets()
    {
        await Task.Delay(100);
        return Database.Wallets.ToArray();
    }

    public async Task<Wallet[]> GetWallets(WalletType walletType)
    {
        await Task.Delay(100);
        return Database.Wallets.Where(x => x.Type == walletType).ToArray();
    }
}
