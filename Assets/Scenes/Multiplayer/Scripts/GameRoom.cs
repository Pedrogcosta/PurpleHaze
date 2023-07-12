using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameRoom : MonoBehaviour
{
    public static string gameName;
    public Button startButton;
    public TMP_Text gameNameText;

    void Awake()
    {
        gameNameText.text = gameName;
        if (CreateGame.created) {
            startButton.interactable = true;
        }       
        SocketManager.Socket.OnUnityThread("promoted", (response) => {
            CreateGame.created = true;
            startButton.interactable = true;
        });
    }

    public void GameStart() {
        SocketManager.Socket.Emit("startGame");
    }
}
