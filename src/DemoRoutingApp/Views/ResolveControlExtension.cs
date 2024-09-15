using Avalonia.Controls;
using System;

namespace DemoRoutingApp.Views;

public class ResolveControlExtension
{
    public Type ControlType { get; set; }

    // note, this IServiceProvider parameter IS NOT related to your configured service provider, and only used for XAML related services
    public Control ProvideValue(IServiceProvider _)
    {
        if (!typeof(Control).IsAssignableFrom(ControlType))
            throw new InvalidOperationException($"The provided type {ControlType} is not a control");

        return (Control)App.ServiceProvider.GetService(ControlType);
    }
}