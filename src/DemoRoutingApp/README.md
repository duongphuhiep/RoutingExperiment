
```js
{
  "path": "/",
  "component": "MainViewModel",
  "children": [
    {
      "path": "wallets",
      "component": "WalletsViewModel",
      "children": [
        {
          "path": ":walletId",
          "component": "WalletDetailViewModel",
          "children": [
            {
              "path": "incoming-transfers",
              "component": "IncomingTransfersViewModel",
              "children": [
                {
                  "path": ":transferId",
                  "component": "TransferDetailSimplifiedViewModel",
                  "children": [
                    {
                      "path": "sender",
                      "component": "WalletDetailSimplifiedViewModel",
                      "children": []
                    }
                  ]
                }
              ]
            },
            {
              "path": "outgoing-transfers",
              "component": "OutgoingTransfersViewModel",
              "children": [
                {
                  "path": ":transferId",
                  "component": "TransferDetailSimplifiedViewModel",
                  "children": [
                    {
                      "path": "receiver",
                      "component": "WalletDetailSimplifiedViewModel",
                      "children": []
                    }
                  ]
                }
              ]
            }
          ]
        }
      ]
    },
    {
      "path": "transfers",
      "component": "TransfersViewModel",
      "children": [
        
      ]
    }
  ]
}
```
