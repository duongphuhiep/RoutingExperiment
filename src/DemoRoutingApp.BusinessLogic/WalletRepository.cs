namespace DemoRoutingApp.BusinessLogic;

public interface IWalletRepository
{
    Task<Wallet?> GetWallet(int id);
    Task<Wallet[]> GetWallets();
}

internal class WalletRepository : IWalletRepository
{
    public async Task<Wallet?> GetWallet(int id)
    {
        await Task.Yield();
        return Database.Wallets.Find(x => x.Id == id);
    }

    public async Task<Wallet[]> GetWallets()
    {
        await Task.Yield();
        return Database.Wallets.ToArray();
    }
}
