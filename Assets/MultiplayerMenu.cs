using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

using SocketIOClient;
using TMPro;

public class MultiplayerMenu : MonoBehaviour
{
    public TextMeshProUGUI chat;
    public TMP_InputField field;
    public SocketIOUnity socket;

     [Serializable]
     public class Chat
     {
         public string username;
         public string message;
     }

     void Awake() {
        GameObject socketManager = GameObject.Find("SocketManager");
        if (socketManager != null) {
            socket = socketManager.GetComponent<LoadManager>().socket;
        } else {
            SceneManager.LoadScene("LoginScene");
        }
     }

    void Start()
    {
        socket.OnUnityThread("chat", (response) => {
            Chat chatMessage = response.GetValue<Chat>();
            chat.text += '\n' + chatMessage.username + ": " + chatMessage.message;
        });
    }

    public void SendChatMessage() {
        socket.Emit("chat", field.text);
        field.text = "";
        field.Select();
    }
}
