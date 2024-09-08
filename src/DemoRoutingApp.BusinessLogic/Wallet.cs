namespace DemoRoutingApp.BusinessLogic;

public enum WalletType { Personal, Company }

public record Wallet
{
    public int Id { get; init; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public WalletType Type { get; set; }

}
