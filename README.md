# Avalonia's app Routing experiments

(At the moment of writing) Navigation & Routing is a [big missing part](https://github.com/AvaloniaUI/Avalonia/discussions/16888) of the [Avalonia's platform](https://avaloniaui.net/).

"navigation" means:

* Navigation History (Mobile + Browser with "push(route)", "replace(route)").
* "Virtual" back button (which bind to the Browser Back button AND Android physical Back button).
* Deep link.

Some community initiative can answer simple navigation requirement but far from enough:

* <https://github.com/AvaloniaInside/Shell>
* <https://github.com/sandreas/Avalonia.SimpleRouter>

Avalonia will support Navigation and Routing in the future, but in the means time, we don't have much.

## What is this repository

This is my playground to find a way to support "Navigation" with what we have today. I will create a simple application but with a really deep interface, and try to navigate between different View elements (Tab, Panel..) in a cross-platform manner. Try to "navigate" to some places deep inside the application, play with navigation history...Adapt the navigation behaviour cross platform..

## Data Model

The application will manage 2 types of entity: Wallet, and Transfer.

* A Wallet has a Name, Category (Personal Wallet, Company Wallet), and a Balance
* A "Transfer" record an "amount" of money movement from a "WalletSender" to a "WalletReceiver"

## Application concepts

The "Home" page should be a TabControls with 3 Tab pages

* Personal Wallets Tab page: containing a list of Personal Wallets
* Company Wallets Tab page: containing a list of Company Wallets
* Transfers Tab page: containing a list of all Transfers

* The "Personal Wallets Tab page" should be assigned to the route "/personalWallets" 
* The "Company Wallets Tab page" should be assigned to the route "/companyWallets" 
  
* both "/personalWallets" and "/companyWallets" would display a list of Wallets, when user click on the Wallet it should display 
  * the Detail of the Wallet
  * the List of Incoming Transfers (transfers which the wallet is receiver)
  * the List of Outgoing Transfers (transfers which the wallet is sender)
 
* when user click on a transfer it would display the detail of the Transfer
...

As you can see, with only 2 entities, and little of codes, we should be able to produce complicate interface with many levels in order to play with Routing and Navigation.

In resume our application can have the following routes

```
/personalWallets
/personalWallets/walletDetails?walletId=1
/personalWallets/walletDetails/incomingTransfers?walletId=1
/personalWallets/walletDetails/incomingTransfers/transferDetails?walletId=1&transferId=1
/personalWallets/walletDetails/incomingTransfers/transferDetails/walletSenderDetails?walletId=1&transferId=1
/personalWallets/walletDetails/outgoingTransfers?walletId=1
/personalWallets/walletDetails/outgoingTransfers/transferDetails?walletId=1&transferId=1
/personalWallets/walletDetails/outgoingTransfers/transferDetails/walletReceiverDetails?walletId=1&transferId=1
/companyWallets
/companyWallets/walletDetails?walletId=1
/companyWallets/walletDetails/incomingTransfers?walletId=1
/companyWallets/walletDetails/incomingTransfers/transferDetails?walletId=1&transferId=1
/companyWallets/walletDetails/incomingTransfers/transferDetails/walletSenderDetails?walletId=1&transferId=1
/companyWallets/walletDetails/outgoingTransfers?walletId=1
/companyWallets/walletDetails/outgoingTransfers/transferDetails?walletId=1&transferId=1
/companyWallets/walletDetails/outgoingTransfers/transferDetails/walletReceiverDetails?walletId=1&transferId=1

/transfers
/transfers/transferDetails?transferId=1
/transfers/transferDetails/walletSenderDetails?transferId=1
/transfers/transferDetails/walletReceiverDetails?transferId=1

/transfers/transferDetails/walletReceiverDetails?transferId=1
```

## View-Model first

I believe that View-Model first is the way to go for big complex application.

//TODO
