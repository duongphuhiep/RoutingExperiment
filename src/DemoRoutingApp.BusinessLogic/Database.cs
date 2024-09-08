namespace DemoRoutingApp.BusinessLogic;

internal class Database
{
    public static readonly List<Wallet> Wallets = new List<Wallet>() {
        new Wallet { Id = 1, Name = "Wallet 1", Balance = 1000, Type=WalletType.Company  },
        new Wallet { Id = 2, Name = "Wallet 2", Balance = 2000, Type=WalletType.Company  },
        new Wallet { Id = 3, Name = "Wallet 3", Balance = 3000, Type=WalletType.Personal },
        new Wallet { Id = 4, Name = "Wallet 4", Balance = 4000, Type=WalletType.Personal  },
    };
    public static readonly List<Transfer> Transfers = new List<Transfer>() {
        new Transfer { Id = 1, WalletSender = 1, WalletReceiver = 2, Amount = 100, Date = DateTime.Now, Comment="Comment 1" },
        new Transfer { Id = 2, WalletSender = 1, WalletReceiver = 2, Amount = 200, Date = DateTime.Now, Comment="Comment 2" },
        new Transfer { Id = 3, WalletSender = 1, WalletReceiver = 3, Amount = 200, Date = DateTime.Now, Comment="Comment 3" },
        new Transfer { Id = 4, WalletSender = 1, WalletReceiver = 3, Amount = 100, Date = DateTime.Now, Comment="Comment 4" },
        new Transfer { Id = 5, WalletSender = 1, WalletReceiver = 4, Amount = 100, Date = DateTime.Now, Comment="Comment 5" },
        new Transfer { Id = 6, WalletSender = 2, WalletReceiver = 1, Amount = 100, Date = DateTime.Now, Comment="Comment 6" },
        new Transfer { Id = 7, WalletSender = 2, WalletReceiver = 1, Amount = 200, Date = DateTime.Now, Comment="Comment 7" },
        new Transfer { Id = 8, WalletSender = 2, WalletReceiver = 3, Amount = 200, Date = DateTime.Now, Comment="Comment 8" },
        new Transfer { Id = 9, WalletSender = 2, WalletReceiver = 3, Amount = 100, Date = DateTime.Now, Comment="Comment 9" },
        new Transfer { Id = 10, WalletSender = 2, WalletReceiver = 4, Amount = 100, Date = DateTime.Now, Comment="Comment 10" },
    };
}
