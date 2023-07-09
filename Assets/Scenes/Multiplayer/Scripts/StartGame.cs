using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartGame : MonoBehaviour
{
    public Button startButton;

    void Awake()
    {
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
