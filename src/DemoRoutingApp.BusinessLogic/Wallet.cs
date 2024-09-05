namespace DemoRoutingApp.BusinessLogic;

public record Wallet
{
    public int Id { get; init; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}
