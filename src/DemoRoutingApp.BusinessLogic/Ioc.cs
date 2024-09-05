using Microsoft.Extensions.DependencyInjection;

namespace DemoRoutingApp.BusinessLogic;

public static class Ioc
{
    /// <summary>
    /// Registration Business Logic
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static ServiceCollection RegisterBusinessLogic(this ServiceCollection services)
    {
        services.AddSingleton<IWalletRepository, WalletRepository>();
        services.AddSingleton<ITransferRepository, TransferRepository>();
        return services;
    }
}
