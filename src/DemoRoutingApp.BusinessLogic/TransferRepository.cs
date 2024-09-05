namespace DemoRoutingApp.BusinessLogic;

public interface ITransferRepository
{
    Task<Transfer?> GetTransfer(int id);
    Task<Transfer[]> GetTransfers();
}

internal class TransferRepository : ITransferRepository
{
    public async Task<Transfer?> GetTransfer(int id)
    {
        await Task.Yield();
        return Database.Transfers.Find(x => x.Id == id);
    }

    public async Task<Transfer[]> GetTransfers()
    {
        await Task.Yield();
        return Database.Transfers.ToArray();
    }
}
