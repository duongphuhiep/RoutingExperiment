namespace DemoRoutingApp.BusinessLogic;

public record Transfer
{
    public int Id { get; init; }
    public int WalletSender { get; set; }
    public int WalletReceiver { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
}
