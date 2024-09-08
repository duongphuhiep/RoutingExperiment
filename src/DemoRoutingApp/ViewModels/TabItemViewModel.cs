namespace DemoRoutingApp.ViewModels;

public record TabItemViewModel
{
    public string? Header { get; set; }
    public ViewModelBase? Content { get; set; }
    public string Name { get; set; }
}
