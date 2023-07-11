using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SocketIOClient;

public class Shop : MonoBehaviour
{
    public class Response {
        public string result;
        public string reason;
        public Dictionary<string, int> items;
        public int coins;
    }

    public static SocketIOResponse response;
    public static Action onClose;

    public TMP_Text totalCoins;
    public TMP_Text swordPrice;
    public TMP_Text heartPrice;
    public TMP_Text speedPrice;
    public Button swordButton;
    public Button heartButton;
    public Button speedButton;

    // Start is called before the first frame update
    void OnEnable()
    {
       var items = response.GetValue<Dictionary<string, int>>(0);
       var coins = response.GetValue<int>(1);

       SetupShop(coins, items);
    }

    void SetupShop(int coins, Dictionary<string, int> items) {
       totalCoins.text = coins.ToString();

       swordPrice.text = items["attack"].ToString();
       heartPrice.text = items["maxHealth"].ToString();
       speedPrice.text = items["speed"].ToString();

       swordButton.interactable = coins >= items["attack"];
       heartButton.interactable = coins >= items["maxHealth"];
       speedButton.interactable = coins >= items["speed"];
    }

    public async void Buy(string item) {
        await SocketManager.Socket.EmitAsync("shop", (response) => {
            var shopResponse = response.GetValue<Response>();
            if (shopResponse.result == "bought") {
                UnityThread.executeInUpdate(() => {
                    SetupShop(shopResponse.coins, shopResponse.items);
                });
            }
        }, item);
    }

    public void Close() {
        if (onClose != null) {
            onClose();
        }
        gameObject.SetActive(false);
    }
}
